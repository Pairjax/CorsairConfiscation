using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;

public class Hook : MonoBehaviour
{
    public GameObject hookedObj;
    public GameObject grabbedObj;
    public GameObject thrownObj;

    private Swingable swingable;
    [SerializeField] private Harpoon harpoon;
    private void Start()
    {
        swingable = GetComponent<Swingable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.childCount == 0 &&
            collision.gameObject.transform.parent != null &&
            collision.transform.parent.parent != null)
        {
            HookObj(collision.transform.parent.parent.gameObject);
        }
    }

    Grabbable grabbableComp = null;
    Throwable throwableComp = null;
    private void HookObj(GameObject selObj)
    {
        if (selObj.tag == "Pathing")
            return;

        throwableComp = selObj.GetComponent<Throwable>();
        grabbableComp = selObj.GetComponent<Grabbable>();

        if (selObj == null || grabbableComp == null || throwableComp != null)
            return;

        if (grabbableComp.isFractured)
            return;

        if (grabbableComp.isGrabbed)
            return;

        grabbableComp.isGrabbed = true;

        hookedObj = selObj;
        grabbedObj = Instantiate(grabbableComp.objectSprite, 
            transform.position, hookedObj.transform.rotation, 
            gameObject.transform).gameObject;
        Rigidbody2D grabbedRigidbody = grabbedObj.GetComponent<Rigidbody2D>();
        grabbedRigidbody.Sleep();
        throwableComp = grabbedObj.AddComponent<Throwable>();
        throwableComp.hook = this;
        throwableComp.settings = grabbableComp.settings;
        hookedObj.SetActive(false);
    }
    public void UnhookObj()
    {
        thrownObj = grabbedObj;
        thrownObj.transform.SetParent(null);
        throwableComp.ApplyPhysics();
        hookedObj = null;
        grabbedObj = null;
        thrownObj = null;
        throwableComp = null;

        harpoon.hState = Harpoon.HookState.Retracting;
    }

    public void UnhookFromFracture()
    {
        thrownObj = grabbedObj;
        thrownObj.transform.SetParent(null);
        Destroy(throwableComp);
        hookedObj = null;
        grabbedObj = null;
        thrownObj = null;
        throwableComp = null;

        harpoon.hState = Harpoon.HookState.Retracting;
    }

    public void ScrapObj()
    {
        // Instantiate(grabbedObj.GetComponent<HookedObjectHolder>().thrownObject, transform.position, Quaternion.identity);
        Salvagable s;
        if (grabbedObj.TryGetComponent<Salvagable>(out s))
            s.OnSalvage();

        Destroy(hookedObj);
        Destroy(grabbedObj);
        hookedObj = null;
        grabbedObj = null;
    }
}
