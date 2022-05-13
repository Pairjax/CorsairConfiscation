using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Harpoon : MonoBehaviour
{
    public PlayerInput input;
    public PlayerStats stats;

    public GameObject root;
    public HingeJoint2D rootJoint;

    public GameObject hookObj;

    public Hook hook;
    public HingeJoint2D hookJoint;

    public int length;

    private void Start()
    {
        length = stats.hookMaxLength;

        rootJoint = root.GetComponent<HingeJoint2D>();
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

    public void Extend()
    {
        length++;

        if (length > stats.hookMaxLength)
        {
            length--;
            Debug.Log("Maximum Length Harpoon Reached!");
            return;
        }

        hookJoint.connectedAnchor += Vector2.right * 2;
    }

    public void Retract()
    {
        length--;

        if (length == 0)
        {
            length = 1;
            gameObject.SetActive(false);
            return;
        }

        hookJoint.connectedAnchor += Vector2.left * 2;
    }

}
