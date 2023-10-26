using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5.0f; // Time in seconds before the projectile is destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // public float speed = 10f;
    // private Rigidbody projectileRb;
    // private GameObject enemy;


    // // private float spawnTime = 5.0f;
    // // private float startDelay = 0.1f;
    // // Start is called before the first frame update

    // // Start is called before the first frame update
    // void Start()
    // {
    //     projectileRb = GetComponent<Rigidbody>();
    //     enemy = GameObject.FindWithTag("Enemy");

    //     ProjectileMovement();
    //     // InvokeRepeating("ProjectileMovement", startDelay, spawnTime);
    // }

    // // Update is called once per frame
    // void Update()
    // {
    // }

    // public void ProjectileMovement()
    // {

    //     Vector3 lookDirection = enemy.transform.position - transform.position;
    //     projectileRb.AddForce(lookDirection * speed);

    //     if (transform.position.y < -10)
    //     { Destroy(gameObject); }

    // }
}