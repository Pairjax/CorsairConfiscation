using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Harpoon : MonoBehaviour
{
    public PlayerInput input;
    public PlayerStats stats;

    public GameObject root;
    public Rigidbody2D rootRb;
    public HingeJoint2D rootJoint;

    public GameObject hookObj;

    public Hook hook;
    public HingeJoint2D hookJoint;

    bool launchMode = false;

    private void Start()
    {
        rootJoint = root.GetComponent<HingeJoint2D>();
        rootRb = root.GetComponent<Rigidbody2D>();
        hookJoint = hook.GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (input.extend)
        {
            Extend();
        }
        else if (input.retract)
        {
            Retract();
        }
    }

    private void FixedUpdate()
    {
        // If we are launching the hook.
        if (launchMode)
            Extend();
    }

    public void Extend()
    {
        float magnitude = hook.transform.localPosition.magnitude;

        if (magnitude > stats.hookMaxLength)
        {
            if (launchMode)
            {
                launchMode = false;
                rootRb.bodyType = RigidbodyType2D.Dynamic;
            }
            return;
        }

        hookJoint.connectedAnchor += Vector2.right * 2;
    }

    public void Retract()
    {
        float magnitude = hook.transform.localPosition.magnitude;

        // If the hook is fully retracted.
        if (magnitude < stats.hookMinLength)
        {
            gameObject.SetActive(false);
            return;
        }

        hookJoint.connectedAnchor += Vector2.left * 2;
    }

    public void Launch()
    {
        launchMode = true;
        rootRb.SetRotation(stats.gameObject.transform.rotation);
        rootRb.bodyType = RigidbodyType2D.Static;
    }

}
