using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{
    private GameObject objInteracting;
    private bool hasInteracted;
    public virtual void Interact(Player player) {
    }
}
