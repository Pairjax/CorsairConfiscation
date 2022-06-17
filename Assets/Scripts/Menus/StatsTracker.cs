using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// God I could only manually code this and I hate myself for that.
public class StatsTracker : MonoBehaviour
{

    [Header("Bounty")]
    public BountyManager bManager;
    public TextMeshProUGUI bountyText;
    public Image bountyImage;
    public Color green;
    public Color yellow;
    public Color red;
    public Sprite safeImg;
    public Sprite unsafeImg;
    public Sprite dangerImg;
    public Sprite deadlyImg;

    [Header("Player Stats")]
    public PlayerStats pStats;

    [Header("Scrap")]
    public TextMeshProUGUI supersteel;
    public TextMeshProUGUI fusion;
    public TextMeshProUGUI laser;
    public TextMeshProUGUI gravity;
    public TextMeshProUGUI ai;
    public TextMeshProUGUI myth;
    public TextMeshProUGUI neutron;

    [Header("Loot")]
    public TextMeshProUGUI hyperfuel;
    public TextMeshProUGUI sensor;
    public TextMeshProUGUI jammer;
    public TextMeshProUGUI skeleton;
    public TextMeshProUGUI impact;
    public TextMeshProUGUI harvester;
    public TextMeshProUGUI aux_cable;
    public TextMeshProUGUI ballast;
    public TextMeshProUGUI card;

    void FixedUpdate()
    {
        UpdateBounty();
        UpdateScrap();
        UpdateLoot();
    }

    private void UpdateBounty()
    {
        int bountyModifier = (bManager.bounty / 1000);
        string bounty = "Bounty: $" + bountyModifier + "K";
        bountyText.text = bounty; 

        if (bountyModifier > 400)
        {
            bountyText.color = red;
            bountyImage.color = red;
            bountyImage.sprite = deadlyImg;
        }
        else if (bountyModifier > 300)
        {
            bountyText.color = red;
            bountyImage.color = red;
            bountyImage.sprite = dangerImg;
        }
        else if (bountyModifier > 200)
        {
            bountyText.color = yellow;
            bountyImage.color = yellow;
            bountyImage.sprite = unsafeImg;
        }
        else
        {
            bountyText.color = green;
            bountyImage.color = green;
            bountyImage.sprite = safeImg;
        }
    }

    private void UpdateScrap()
    {
        GenerateText(supersteel, 0);
        GenerateText(fusion, 0);
        GenerateText(laser, 0);
        GenerateText(gravity, 0);
        GenerateText(ai, 0);
        GenerateText(myth, 0);
        GenerateText(neutron, 0);

        foreach (KeyValuePair<Droppable, int> d in pStats.scrap)
        {
            switch (d.Key.name)
            {
                case "Supersteel":
                    GenerateText(supersteel, d.Value);
                    break;
                case "Fusion Coil":
                    GenerateText(fusion, d.Value);
                    break;
                case "Laser Battery":
                    GenerateText(laser, d.Value);
                    break;
                case "Gravity Disks":
                    GenerateText(gravity, d.Value);
                    break;
                case "AI Fragments":
                    GenerateText(ai, d.Value);
                    break;
                case "Mythium Alloy":
                    GenerateText(myth, d.Value);
                    break;
                case "Neutron Wiring":
                    GenerateText(neutron, d.Value);
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateLoot()
    {
        GenerateText(hyperfuel, 0);
        GenerateText(sensor, 0);
        GenerateText(jammer, 0);
        GenerateText(skeleton, 0);
        GenerateText(impact, 0);
        GenerateText(harvester, 0);
        GenerateText(aux_cable, 0);
        GenerateText(ballast, 0);
        GenerateText(card, 0);

        foreach (KeyValuePair<Droppable, int> d in pStats.loot)
        {
            switch (d.Key.name)
            {
                case "Hyperfuel":
                    GenerateText(hyperfuel, d.Value);
                    break;
                case "Advanced Sensor":
                    GenerateText(sensor, d.Value);
                    break;
                case "SPF Signal Jammer":
                    GenerateText(jammer, d.Value);
                    break;
                case "Improved Hull Skeleton":
                    GenerateText(skeleton, d.Value);
                    break;
                case "Impact Dampener":
                    GenerateText(impact, d.Value);
                    break;
                case "Modulated Harvesters":
                    GenerateText(harvester, d.Value);
                    break;
                case "Auxiliary Service Cable":
                    GenerateText(aux_cable, d.Value);
                    break;
                case "Starship Ballast":
                    GenerateText(ballast, d.Value);
                    break;
                case "Rest-Station Rewards Card":
                    GenerateText(card, d.Value);
                    break;
                default:
                    break;
            }
        }
    }

    private void GenerateText(TextMeshProUGUI textBox, int value)
    {
        textBox.text = value + "X";
    }
}
