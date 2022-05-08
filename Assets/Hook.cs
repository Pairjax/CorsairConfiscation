using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject hookedObj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("grabbable") && transform.childCount == 0)
        {
            HookObj(collision.gameObject);
        }
    }

    private void HookObj(GameObject selObj)
    {
        // There needs to be a better system for this, since it's a prototype we can ignore for now. -Tyler
        CopShipController controller = selObj.GetComponent<CopShipController>();
        if (controller != null) controller.shipState = CopShipController.ShipState.Disabled;

        CivilianShipController civController = selObj.GetComponent<CivilianShipController>();
        if (civController != null) civController.SetState(CivilianShipController.ShipState.Disabled);
        Debug.Log(selObj.name);
        if (selObj.name.Equals("Sprite") && selObj.transform.parent.gameObject.GetComponent<Asteroid>() != null)
            selObj = selObj.transform.parent.gameObject;
        
        selObj.transform.SetParent(transform);
        hookedObj = selObj;
    }

    public void UnhookObj()
    {
        if (hookedObj == null)
        {
            // Sets whole harpoon to unactive.
            gameObject.transform.parent.gameObject.SetActive(false);
            return;
        }

        // Same goes for unhooking.
        CopShipController controller = hookedObj.GetComponent<CopShipController>();
        if (controller != null) controller.ResetAI();

        CivilianShipController civController = hookedObj.GetComponent<CivilianShipController>();
        if (civController != null) civController.SetState(CivilianShipController.ShipState.Moving);

        if (hookedObj)
        {
            hookedObj.transform.SetParent(null);
            hookedObj = null;
        }
        // Sets whole harpoon to unactive.
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
