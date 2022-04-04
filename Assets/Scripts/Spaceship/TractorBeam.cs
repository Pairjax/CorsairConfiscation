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
        if (!hasObject) return;

        targetObj.transform.position = tractorTip.position;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name.Equals("Despawner")) return;
        if (collider.name.Equals("Sprite")) return;
        if (collider.name.Equals("PlayerCheck")) return;
        if (collider.GetComponent<ObjectSpawner>()) return;

        GameObject parentObj = collider.gameObject;
        if (parentObj.tag != "grabbable" && collider.gameObject.tag != "grabbable")
        {
            Debug.Log($"Returned; {parentObj.tag}, {parentObj.name}, and {collider.gameObject.tag},  {collider.gameObject.name}");
            return;
        }

        CopShipController controller = parentObj.GetComponent<CopShipController>();
        if (controller != null) controller.shipState = CopShipController.ShipState.Disabled;

        CivilianShipController civController = parentObj.GetComponent<CivilianShipController>();
        if (civController != null) civController.SetState(CivilianShipController.ShipState.Disabled);
        
        Transform targetT = collider.gameObject.transform;
        if (hasObject)
        {
            float distToShip = Vector2.Distance(targetT.position, playerLoc.position);
            if (distToShip > 1f)
            {
                tractorTip.position = Vector3.MoveTowards(tractorTip.position, playerLoc.position, 0.5f * Time.deltaTime);
            }
            else
            {
                UIManager.instance.UpdateGotchaCounterUI(1);
                Destroy(collider.gameObject);
                tractorTip.position = initTip.position;
                hasObject = false;
            }
            return;
        }

        tractorLoc = new Vector2(tractorTip.position.x, tractorTip.position.y);
        shipLoc = new Vector2(targetT.position.x, targetT.position.y);

        if (!hasObject)
        {
            if (shipLoc != tractorLoc)
            {
                Vector2 newLocation = Vector2.Lerp(shipLoc, tractorLoc, 0.1f);
                targetT.position = new Vector3(newLocation.x, newLocation.y, targetT.position.z);
            }

            float distance = Vector2.Distance(targetT.position, tractorTip.position);
            print(distance);
            if (distance < 0.8f)
            {
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
        if (copShip) copShip.ResetAI();

        CivilianShipController civShip = targetObj.GetComponent<CivilianShipController>();
        if (civShip) civShip.SetState(CivilianShipController.ShipState.Moving);
            
        targetObj = null;
    }
}
