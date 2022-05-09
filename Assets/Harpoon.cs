using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Harpoon : MonoBehaviour
{
    public PlayerInput input;

    public GameObject root;
    public GameObject hookObj;

    public Hook hook;

    public List<GameObject> rungs;

    private void Start()
    {
        rungs.Add(root);
        rungs.Add(hookObj);
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

    public void Extend()
    {
        // Create new rung.
        GameObject newRung = Instantiate(root);
        newRung.transform.parent = gameObject.transform;

        // Reassemble joints in adjacent rungs.
        HingeJoint2D joint = newRung.GetComponent<HingeJoint2D>();
        Rigidbody2D rb = newRung.GetComponent<Rigidbody2D>();
        joint.connectedBody = rungs[0].GetComponent<Rigidbody2D>();
        rungs[1].GetComponent<HingeJoint2D>().connectedBody = rb;

        joint.connectedAnchor = new Vector2(-1, 0);

        // Reassemble list to track current rungs.
        List<GameObject> newRungs = new List<GameObject>();

        newRungs.Add(rungs[0]);
        newRungs.Add(newRung);

        for (int i = 1; i < rungs.Count; i++)
        {
            newRungs.Add(rungs[i]);
        }

        rungs = newRungs;
    }

    public void Retract()
    {
        if (rungs.Count <= 2)
        {
            hook.UnhookObj();
            return;
        }

        // Reassemble joints in adjacent rungs.
        HingeJoint2D jointSecond = rungs[2].GetComponent<HingeJoint2D>();
        Rigidbody2D rbFirst = rungs[0].GetComponent<Rigidbody2D>();

        jointSecond.connectedBody = rbFirst;

        GameObject undoRung = rungs[1];
        rungs.Remove(undoRung);

        Destroy(undoRung);
    }

}
