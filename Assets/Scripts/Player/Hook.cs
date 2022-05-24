using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private enum HookState
    {
        Unlaunched,
        Launching,
        Retracting,
        Launched
    }

    [SerializeField] private HookState hState = HookState.Unlaunched;
    private List<GameObject> hookRungs;
    [SerializeField] private float ropeLength;

    public GameObject hookedObj;
    public GameObject grabbedObj;

    #region Launcher

    private void FixedUpdate()
    {
        if (hState == HookState.Launched
            || hState == HookState.Unlaunched)
            return;
        else if (hState == HookState.Launching)
        {
            transform.localPosition += Vector3.left * Time.fixedDeltaTime;

            if (Vector3.Distance(transform.parent.position, transform.position) > ropeLength)
                hState = HookState.Retracting;
        }
        else if (hState == HookState.Retracting)
        {
            transform.localPosition += Vector3.right * Time.fixedDeltaTime;

            if (Vector3.Distance(transform.parent.position, transform.position) < 0.2f)
                hState = HookState.Unlaunched;
        }
    }

    public bool OnLaunchHook()
    {
        if (hState != HookState.Unlaunched)
            return false;

        hState = HookState.Launching;

        return true;
    }

    private void ActivateRope()
    {
        // TODO: Create rungs evenly spaced out from hook's current position.

    }

    #endregion

    #region Grabber

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.childCount == 0)
        {
            HookObj(collision.gameObject);
        }
    }

    private void HookObj(GameObject selObj)
    {
        Grabbable grabbableComp = selObj.GetComponent<Grabbable>();

        if (grabbableComp == null)
        {
            selObj = selObj.transform.parent.gameObject;
            grabbableComp = selObj.GetComponent<Grabbable>();
        }

        if (selObj == null || grabbableComp == null)
            return;

        hookedObj = selObj;
        grabbedObj = Instantiate(grabbableComp.objectSprite, transform.position, hookedObj.transform.rotation, gameObject.transform).gameObject;
        hookedObj.SetActive(false);
    }

    public void UnhookObj()
    {
        if (!hookedObj && !grabbedObj)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            return;
        }
        Instantiate(grabbedObj.GetComponent<HookedObjectHolder>().thrownObject, transform.position, Quaternion.identity);
        Destroy(grabbedObj);
        hookedObj = null;
        grabbedObj = null;
        // Sets whole harpoon to unactive.
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    #endregion
}
