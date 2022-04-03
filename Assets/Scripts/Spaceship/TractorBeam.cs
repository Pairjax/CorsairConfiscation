using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Transform tractorTip;
    Vector2 shipLoc;
    Vector2 tractorLoc;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "grabbable")
        {

            GameObject parentObj = collider.gameObject;
            Debug.Log(parentObj.name);

            CopShipController controller = parentObj.GetComponent<CopShipController>();
            if (controller != null) controller.shipState = CopShipController.ShipState.Disabled;

            Transform targetT = collider.gameObject.transform;
            Debug.Log("ship grabbed");
            tractorLoc = new Vector2(tractorTip.position.x, tractorTip.position.y);
            shipLoc = new Vector2(targetT.position.x, targetT.position.y);

            if (shipLoc != tractorLoc)
            {
                Vector2 newLocation = Vector2.Lerp(shipLoc, tractorLoc, 0.5f);
                targetT.position = new Vector3(newLocation.x, newLocation.y, targetT.position.z);
            }
        }
      

    }
}
