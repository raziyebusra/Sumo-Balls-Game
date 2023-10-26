using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerUpStrenght = 10.0f;
    public float speed = 5.0f;
    public bool hasPowerup = false;
    public bool hasGreenPowerup = false;
    public GameObject powerupIndicator;
    // public GameObject projectiles;

    //public Projectile projectileScript;

    private GameObject projectilePrefab;
    private int numberOfProjectiles;

    public enum ProjectileDirection
    {
        Forward,
        Left,
        Right,
        Backward
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
        else if (other.CompareTag("GreenPowerup"))
        {
            hasGreenPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
            SpawnProjectile();
            //    SpawnProjectile(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        hasGreenPowerup = false;
        powerupIndicator.gameObject.SetActive(false);

    }

    public void ActivatePowerUp(GameObject projectilePrefab, int numProjectiles)
    {
        this.projectilePrefab = projectilePrefab;
        numberOfProjectiles = numProjectiles;
    }
    public void SpawnProjectile()
    {

        if (Input.GetKeyDown(KeyCode.Space) && projectilePrefab != null)
        {
            // Define the angles for each projectile direction (in degrees)
            float[] angles = { 0f, 90f, -90f, 180f }; // Forward, Right, Left, Backward

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                int directionIndex = i % angles.Length; // Reuse directions if there are more projectiles
                float angle = angles[directionIndex];
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 spawnPosition = transform.position + rotation * Vector3.forward;
                GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, rotation);
            }
            //     if (Input.GetKeyDown(KeyCode.Space))
            //     {
            //         Projectile pScript = projectiles.GetComponent<Projectile>();

            //         pScript.ProjectileMovement();
            //         Instantiate(projectiles, transform.position, projectiles.transform.rotation);
            //     }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrenght, ForceMode.Impulse);

            Debug.Log("collided with: " + collision.gameObject.name + "with powerup set to" + hasPowerup);
        }
    }


}
