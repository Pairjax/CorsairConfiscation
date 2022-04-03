using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Transform tractorTip;
    public Transform playerLoc;
    public Transform initTip;
    Vector2 shipLoc;
    Vector2 tractorLoc;

    private bool hasObject = false;
    private GameObject targetObj;

    [SerializeField] private Transform environment;

    private void FixedUpdate()
    {
        Debug.Log(hasObject);
        if (!hasObject) return;

        targetObj.transform.position = tractorTip.position;

    }

    public void OnTriggerStay2D(Collider2D collider)
    {
      

        GameObject parentObj = collider.gameObject;
        // ITS NOT DETECTING THE CIV SHIPS HERE AS HAVING GRABBALE
        if (parentObj.tag != "grabbable" && collider.gameObject.tag != "grabbable")
        {
            Debug.Log("returned");
            return;
        }
            

        Debug.Log(parentObj.name);

        CopShipController controller = parentObj.GetComponent<CopShipController>();
        if (controller != null) controller.shipState = CopShipController.ShipState.Disabled;

        Transform targetT = collider.gameObject.transform;
       //   Debug.Log(targetT.gameObject.name);
        if (hasObject)
        {
            Debug.Log("Ship pulled");
            float distToShip = Vector3.Distance(targetT.position, playerLoc.position);
            if (distToShip > 1f)
            {
                Debug.Log("Ship pulling");
                tractorTip.position = Vector3.MoveTowards(tractorTip.position, playerLoc.position, 0.5f * Time.deltaTime);
            }
            else
            {
                Debug.Log("Ship gotchad");
                Destroy(collider.gameObject);
                tractorTip.position = initTip.position;
            }
            return;
        }

        Debug.Log("ship grabbed");
        tractorLoc = new Vector2(tractorTip.position.x, tractorTip.position.y);
        shipLoc = new Vector2(targetT.position.x, targetT.position.y);

        if (!hasObject)
        {
            if (shipLoc != tractorLoc)
            {
                Vector2 newLocation = Vector2.Lerp(shipLoc, tractorLoc, 0.1f);
                targetT.position = new Vector3(newLocation.x, newLocation.y, targetT.position.z);
            }

            float distance = Vector3.Distance(targetT.position, tractorTip.position);
            print(distance);
            if (distance < 0.8f)
            {
                Debug.Log("has object");
                targetT.parent = tractorTip;
                targetObj = parentObj;
                hasObject = true;
            }
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
