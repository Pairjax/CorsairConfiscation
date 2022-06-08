using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Purchase : MonoBehaviour, IPointerClickHandler
{
    public ShipComponent item;
    public PlayerStats pStats;

    public void OnPointerClick(PointerEventData eventData)
    {
        pStats.AddComponent(item);
    }
}
