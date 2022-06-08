using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject hookedObj;
    public GameObject grabbedObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.childCount == 0)
        {
            HookObj(collision.gameObject);
        }
    }

    private void HookObj(GameObject selObj)
    {
        if (selObj.tag == "Pathing")
            return;

        Grabbable grabbableComp = selObj.GetComponent<Grabbable>();

        if (grabbableComp == null)
        {
            selObj = selObj.transform.parent.gameObject;
            grabbableComp = selObj.GetComponent<Grabbable>();
        }

        if (selObj == null || grabbableComp == null)
            return;

        hookedObj = selObj;
        grabbedObj = Instantiate(grabbableComp.objectSprite, 
            transform.position, hookedObj.transform.rotation, 
            gameObject.transform).gameObject;
        hookedObj.SetActive(false);
    }

    public void UnhookObj()
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
