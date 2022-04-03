using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Transform tractorTip;
    Vector2 shipLoc;
    Vector2 tractorLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "grabbable");
        {
            Debug.Log(collider.transform.parent.gameObject.name);
            collider.transform.parent.gameObject.GetComponent<CopShipController>().shipState = CopShipController.ShipState.Disabled;
            
            Debug.Log("ship grabbed");
            tractorLoc = new Vector2(tractorTip.position.x, tractorTip.position.y);
            shipLoc = new Vector2(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y);

            if (shipLoc != tractorLoc)
            {
                collider.gameObject.transform.position = Vector2.Lerp(shipLoc, tractorLoc, 0.5f);
            }
        }
      

    }
}
