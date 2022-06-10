using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShipComponent component;

    [SerializeField] private Image icon;
    [SerializeField] private Image componentTypeIcon;

    [SerializeField] private Image scrap1;
    [SerializeField] private TextMeshProUGUI scrap1Text;
    [SerializeField] private Image scrap2;
    [SerializeField] private TextMeshProUGUI scrap2Text;
    [SerializeField] private Image scrap3;
    [SerializeField] private TextMeshProUGUI scrap3Text;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
