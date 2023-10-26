using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2.0f;
    public float angle = 0f; // Angle at which the projectile should move (in degrees)

    private void Start()
    {
        Destroy(gameObject, lifetime);
        // Convert the angle to radians for trigonometric calculations
        float angleRad = Mathf.Deg2Rad * angle;

        // Calculate the direction vector from the angle
        Vector3 moveDirection = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));

        // Set the velocity based on the direction
        GetComponent<Rigidbody>().velocity = moveDirection * speed;
    }
}