using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salvagable : MonoBehaviour
{
    public SpaceshipStats stats;
    [SerializeField] private GameObject dropReference;

    public void OnSalvage()
    {
        foreach (DropParameters drop in stats.drops)
        {
            GameObject dropObj = Instantiate(dropReference);
            dropObj.SetActive(false);

            Collectible dropC = dropObj.AddComponent<Collectible>();
            dropC.drop = drop.drop;

            Rigidbody2D dropRb2d = dropObj.AddComponent<Rigidbody2D>();

            dropObj.transform.position = gameObject.transform.position;

            dropObj.SetActive(true);

            dropRb2d.AddForce(Random.insideUnitCircle * ((Random.value * 80) + 40));
        }
    }
}
