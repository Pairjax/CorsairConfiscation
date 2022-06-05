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
        UpdateRope();

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
                hookSwing.radius += 4 * Time.fixedDeltaTime;
        }
        else if (hState == HookState.Retracting)
        {
            hookSwing.radius -= 4 * Time.fixedDeltaTime;
        }

        if (hook.hookedObj != null
                || hook.grabbedObj != null)
        {
            ActivateRope();
            hState = HookState.Launched;
            return;
        }
    }

    public void OnRetract()
    {
        hookSwing.radius -= 0.05f;
    }

    public void OnExtend()
    {
        hookSwing.radius += 0.05f;
    }

    public bool OnLaunchHook()
    {
        if (onCooldown) return false;
        if (hState != HookState.Unlaunched) return false;

        StartCoroutine(TimerRoutine());

        hState = HookState.Launching;
        hookObj.SetActive(true);
        harpoonRope.target = hookObj;

        return true;
    }
    private IEnumerator TimerRoutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(pStats._hookCooldown);
        onCooldown = false;
    }

    private void UpdateRope()
    {
        float distance = Vector3.Distance(hookT.position, transform.position);
        float newIncrement = distance / (float)Mathf.Max(1, ropeRungs.Count);

        for (int i = 0; i < ropeRungs.Count; i++)
        {
            Swingable rung = ropeRungs[i].GetComponent<Swingable>();
            rung.radius = newIncrement * (i + 1);
        }
    }

    private void ActivateRope()
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

    // Gives time for ropeSegments to move into proper place before rendering
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
