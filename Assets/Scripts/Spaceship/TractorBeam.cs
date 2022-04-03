using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Transform tractorTip;
    Vector2 shipLoc;
    Vector2 tractorLoc;

    private bool hasObject = false;
    private GameObject targetObj;

    [SerializeField] private Transform environment;

    private void FixedUpdate()
    {
        if (!hasObject) return;

        targetObj.transform.position = tractorTip.position;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (hasObject) return;

        GameObject parentObj = collider.gameObject;
        if (parentObj.tag != "grabbable") return;

        Debug.Log(parentObj.name);

        CopShipController controller = parentObj.GetComponent<CopShipController>();
        if (controller != null) controller.shipState = CopShipController.ShipState.Disabled;

        Transform targetT = collider.gameObject.transform;
        Debug.Log("ship grabbed");
        tractorLoc = new Vector2(tractorTip.position.x, tractorTip.position.y);
        shipLoc = new Vector2(targetT.position.x, targetT.position.y);

        if (shipLoc != tractorLoc)
        {
            Vector2 newLocation = Vector2.Lerp(shipLoc, tractorLoc, 0.1f);
            targetT.position = new Vector3(newLocation.x, newLocation.y, targetT.position.z);
        }

        float distance = Vector3.Distance(targetT.position, tractorTip.position);
        print(distance);
        if (distance < 0.8f)
        {
            targetT.parent = tractorTip;
            targetObj = parentObj;
            hasObject = true;
        }
    }

    public void Release()
    {
        if (!hasObject) return;
        hasObject = false;
        targetObj.transform.parent = environment;

        CopShipController copShip = targetObj.GetComponent<CopShipController>();
        if (copShip)
        {
            copShip.SetState(CopShipController.ShipState.Idle);
        }
        targetObj = null;
    }
}
