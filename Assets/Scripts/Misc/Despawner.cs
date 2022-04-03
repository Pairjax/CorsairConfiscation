using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        switch(other.gameObject.name)
        {
            case "Bullet(Clone)":
                Destroy(other.gameObject);
                break;
            case "Sprite":
                Destroy(other.gameObject.transform.parent.gameObject);
                break;
        }    
    }
}


