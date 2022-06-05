using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    private enum HookState
    {
        Unlaunched,
        Launching,
        Retracting,
        Launched
    }

    [SerializeField] private GameObject player;
    private PlayerStats pStats;

    [SerializeField] private GameObject hookObj;
    private Transform hookT;
    private Hook hook;
    private Swingable hookSwing;

    public bool onCooldown { private set; get; }

    private RopeSegment harpoonRope;
    private List<GameObject> ropeRungs = new List<GameObject>();
    [SerializeField] private GameObject rungPrefab;

    private HookState hState = HookState.Unlaunched;

    private void Start()
    {
        hookT = hookObj.GetComponent<Transform>();
        hook = hookObj.GetComponent<Hook>();
        hookSwing = hookObj.GetComponent<Swingable>();

        pStats = player.GetComponent<PlayerStats>();

        harpoonRope = GetComponent<RopeSegment>();
    }

    private void Update()
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (hState == HookState.Launched)
            UpdateRopeStretch();
        HandleState();
    }

    private void UpdateRopeStretch()
    {
        float distance = Vector3.Distance(hookT.position, transform.position);
        float newIncrement = distance / (float)Mathf.Max(1, ropeRungs.Count);

        for (int i = 0; i < ropeRungs.Count; i++)
        {
            Swingable rung = ropeRungs[i].GetComponent<Swingable>();
            rung.radius = newIncrement * (i + 1);
        }
    }

    private void HandleState()
    {
        if (hState == HookState.Unlaunched)
            return;

        float distance = Vector3.Distance(hookT.position, transform.position);

        if (hState == HookState.Launched
            || hState == HookState.Retracting)
        {
            if (distance < 0.2f)
            {
                DisableRope();
                hState = HookState.Unlaunched;
                return;
            }
        }
        if (hState == HookState.Launched)
        {
            if (distance > pStats._hookMaxLength)
                hookSwing.radius = pStats._hookMaxLength;

            return;
        }
        if (hState == HookState.Launching)
        {
            if (distance > pStats._hookMaxLength)
                hState = HookState.Retracting;
            else
                Extend();
        }
        else if (hState == HookState.Retracting)
        {
            Retract();
        }

        if (hook.hookedObj != null
                || hook.grabbedObj != null)
        {
            ConvertToSwingable();
            hState = HookState.Launched;
            return;
        }
    }

    public void Retract()
    {
        hookSwing.radius -= pStats._hookLaunchSpeed * Time.fixedDeltaTime;
    }

    public void Extend()
    {
        hookSwing.radius += pStats._hookLaunchSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Launches the hook when the ability is available.
    /// </summary>
    /// <returns>If the harpoon successfuly launched.</returns>
    public bool OnLaunchHook()
    {
        if (onCooldown) return false;
        if (hState != HookState.Unlaunched) return false;

        StartCoroutine(StartHookCooldown());

        hState = HookState.Launching;
        hookObj.SetActive(true);
        harpoonRope.target = hookObj;

        return true;
    }

    private IEnumerator StartHookCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(pStats._hookCooldown);
        onCooldown = false;
    }

    /// <summary>
    /// Activates when the rope has caught a Grabbable.
    /// Converts simple rope into a set of Swingable rungs.
    /// </summary>
    private void ConvertToSwingable()
    {
        foreach (GameObject rope in ropeRungs)
            Destroy(rope);

        ropeRungs.Clear();

        int numRungs = Mathf.RoundToInt(pStats._hookMaxLength * 2);
        float angleIncrement = hookSwing.angleLimit / (float)Mathf.Max(1, numRungs);
        for (int i = 0; i < numRungs; i++)
        {
            GameObject rung = Instantiate(rungPrefab);
            rung.SetActive(false);
            Swingable rungSwing = rung.GetComponent<Swingable>();

            rungSwing.radius = i * 0.5f;
            rungSwing.angleLimit = angleIncrement * (i + 1);
            rungSwing.origin = player;

            rung.transform.SetParent(transform);

            ropeRungs.Add(rung);
        }

        harpoonRope.target = gameObject;
        foreach (GameObject r in ropeRungs)
        {
            RopeSegment rSegment = r.GetComponent<RopeSegment>();
            rSegment.target = r;
            r.SetActive(true);
        }

        hookObj.SetActive(true);

        StartCoroutine(VisualizeRope());
    }

    // Necessary to let rope rungs move into place before being rendered.
    private IEnumerator VisualizeRope()
    {
        yield return new WaitForSeconds(0.02f);

        harpoonRope.target = ropeRungs[0];
        for (int i = 0; i < ropeRungs.Count - 1; i++)
        {
            RopeSegment rSegment = ropeRungs[i].GetComponent<RopeSegment>();
            rSegment.target = ropeRungs[i + 1];
        }
        ropeRungs[ropeRungs.Count - 1].GetComponent<RopeSegment>().target = hookObj;
    }

    private void DisableRope()
    {
        foreach (GameObject rope in ropeRungs)
            Destroy(rope);

        ropeRungs.Clear();

        if (hook.hookedObj != null
                || hook.grabbedObj != null)
            hook.UnhookObj();

        harpoonRope.target = hookObj;
        hookObj.SetActive(false);

        harpoonRope.target = gameObject;
    }
}
