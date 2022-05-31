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

    [SerializeField] private GameObject hookObj;
    private Transform hookT;
    private Hook hook;
    private Swingable hookSwing;

    private RopeSegment rope;
    [SerializeField] private List<GameObject> ropeRungs;
    [SerializeField] private GameObject rungPrefab;

    [SerializeField] private HookState hState = HookState.Unlaunched;
    [SerializeField] private float ropeLength = 5;

    private void Start()
    {
        hookT = hookObj.GetComponent<Transform>();
        hook = hookObj.GetComponent<Hook>();
        hookSwing = hookObj.GetComponent<Swingable>();

        rope = GetComponent<RopeSegment>();
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
            if (distance > ropeLength)
                hookSwing.radius = ropeLength;

            return;
        }
        if (hState == HookState.Launching)
        {
            if (distance > ropeLength)
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
        if (hState != HookState.Unlaunched)
            return false;

        hState = HookState.Launching;
        hookObj.SetActive(true);
        rope.target = hookObj;

        return true;
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
        {
            Destroy(rope);
        }

        ropeRungs.Clear();

        int numRungs = Mathf.RoundToInt(ropeLength * 2);
        float angleIncrement = hookSwing.angleLimit / (float)Mathf.Max(1, numRungs);
        for (int i = 0; i < numRungs; i++)
        {
            GameObject rung = Instantiate(rungPrefab);
            rung.SetActive(false);
            Swingable rungSwing = rung.GetComponent<Swingable>();

            rungSwing.radius = (i + 1) * 0.5f;
            rungSwing.angleLimit = angleIncrement * (i + 1);
            rungSwing.origin = player;

            rung.transform.SetParent(transform);

            ropeRungs.Add(rung);
        }

        GetComponent<RopeSegment>().target = ropeRungs[0];

        for (int i = 0; i < ropeRungs.Count - 1; i++)
        {
            RopeSegment rSegment = ropeRungs[i].GetComponent<RopeSegment>();
            rSegment.target = ropeRungs[i + 1];
            ropeRungs[i].SetActive(true);
        }

        ropeRungs[ropeRungs.Count - 1].GetComponent<RopeSegment>().target = hookObj;
        ropeRungs[ropeRungs.Count - 1].SetActive(true);

        hookObj.SetActive(true);
    }

    private void DisableRope()
    {
        foreach (GameObject rope in ropeRungs)
            Destroy(rope);

        ropeRungs.Clear();

        if (hook.hookedObj != null
                || hook.grabbedObj != null)
            hook.UnhookObj();

        rope.target = hookObj;
        hookObj.SetActive(false);

        rope.target = gameObject;
    }
}
