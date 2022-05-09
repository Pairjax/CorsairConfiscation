using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject hookedObj;
    public GameObject grabbedObj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.childCount == 0)
        {
            HookObj(collision.gameObject);
        }
    }

    private void HookObj(GameObject selObj)
    {
        Grabbable grabbableComp = selObj.GetComponent<Grabbable>();

        if(grabbableComp == null)
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
        if(!hookedObj && !grabbedObj)
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
}
