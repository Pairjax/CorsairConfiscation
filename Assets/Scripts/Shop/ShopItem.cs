using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShipComponent component;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI effect;

    [SerializeField] private Image icon;
    [SerializeField] private Image componentTypeIcon;

    [SerializeField] private Image scrap1;
    [SerializeField] private TextMeshProUGUI scrap1Text;
    [SerializeField] private Image scrap2;
    [SerializeField] private TextMeshProUGUI scrap2Text;
    [SerializeField] private Image scrap3;
    [SerializeField] private TextMeshProUGUI scrap3Text;

    public PlayerStats pStats;

    void Start()
    {
        ApplyComponentDetails();
    }

    public void Refresh(ShipComponent c)
    {
        component = c;
        ApplyComponentDetails();
    }

    private void ApplyComponentDetails()
    {
        icon.sprite = component.sprite;

        title.text = component.name;
        description.text = component.description;
        if (effect != null)
            effect.text = component.effect;

        scrap1.sprite = component.price[0].drop.sprite;
        scrap1Text.text = component.price[0].quantity + "X";

        if (component.price.Count < 2)
        {
            scrap2.color = Color.clear;
            scrap2Text.text = "";
        }
        else
        {
            scrap2.color = Color.white;
            scrap2.sprite = component.price[1].drop.sprite;
            scrap2Text.text = component.price[1].quantity + "X";
        }

        if (component.price.Count < 3)
        {
            scrap3.color = Color.clear;
            scrap3Text.text = "";
        }
        else
        {
            scrap3.color = Color.white;
            scrap3.sprite = component.price[2].drop.sprite;
            scrap3Text.text = component.price[2].quantity + "X";
        }
    }

    public void OnPurchase()
    {
        foreach (CostParameters cost in component.price)
        {
            if (!pStats.scrap.ContainsKey(cost.drop)
                || pStats.scrap[cost.drop] < cost.quantity)
            {
                Debug.Log("Not enough scrap to purchase!");
                return;
            }
        }

        foreach (CostParameters cost in component.price)
            pStats.scrap[cost.drop] -= cost.quantity;

        pStats.AddComponent(component);
    }
}
