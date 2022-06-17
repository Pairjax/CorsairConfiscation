using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    public float rotateSpeed;
    public Vector3 destination;
    private void Start()
    {
        LookAt();
        rb2d.isKinematic = true;
        rb2d.velocity = transform.TransformDirection(Vector3.right * speed);
    }
    void Update()
    {
        if (!transform.childCount.Equals(0))
            transform.GetChild(0).transform.Rotate(0, 0, rotateSpeed * Time.deltaTime); //rotates 50 degrees per second around z axis
        else
            Destroy(gameObject);
    }
    private void LookAt()
    {
        Vector3 dir = destination - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
