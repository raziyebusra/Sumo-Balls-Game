using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerUpStrenght = 10.0f;
    public float speed = 5.0f;

    //explosion radius
    public float radius = 50.0F;
    //explosion force
    public float power = 3000.0F;

    public bool hasPowerup = false;
    public bool isGreenPowerupActive = false;
    public bool isAttackPowerupActive = false;

    private bool isMovingToTarget = false; // Indicates if the object is currently moving to the target in SmashAttack
    private bool isGrounded = true;
    private bool jumped = false;



    private Coroutine powerupCountdown;

    public GameObject powerupIndicator;
    public GameObject powerupIndicatorGreen;
    public GameObject powerupIndicatorAttack;


    public GameObject projectilePrefab;
    public float projectileSpawnDistance = 0.5f; // Adjust this value to control the spawn distance



    //SmashAttack variables          
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private object enemies;

    //public Projectile projectileScript;


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
        powerupIndicatorGreen.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicatorAttack.transform.position = transform.position + new Vector3(0, -0.5f, 0);


        if (isGreenPowerupActive && Input.GetKeyDown(KeyCode.Space))
        {
            SpawnProjectile();
        }

        if (isAttackPowerupActive && Input.GetKeyDown(KeyCode.Space) && !isMovingToTarget && isGrounded)
        {
            StartCoroutine(SmashAttack());
            StartCoroutine(FreezeXYPositions());
            jumped = true;
            isGrounded = false;


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            if (!hasPowerup) // Check if the player doesn't already have a powerup
            {
                hasPowerup = true;
                Destroy(other.gameObject);
                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown); // Stop the existing countdown coroutine 
                    isAttackPowerupActive = false; // set other powerups inactive
                    isGreenPowerupActive = false;
                    powerupIndicatorGreen.gameObject.SetActive(false);
                    powerupIndicatorAttack.gameObject.SetActive(false);

                }

                powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
                powerupIndicator.gameObject.SetActive(true);
            }
        }
        else if (other.CompareTag("GreenPowerup"))
        {
            if (!isGreenPowerupActive) // Check if the green powerup is not already active
            {
                isGreenPowerupActive = true;
                Destroy(other.gameObject);

                if (powerupCountdown != null)
                {
                    StopCoroutine(powerupCountdown); // Stop the existing countdown coroutine
                    isAttackPowerupActive = false; // set other powerups inactive
                    hasPowerup = false;
                    powerupIndicator.gameObject.SetActive(false);
                    powerupIndicatorAttack.gameObject.SetActive(false);

                }

                powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
                powerupIndicatorGreen.gameObject.SetActive(true);

            }
        }
        else if (other.CompareTag("AttackPowerup"))
        {
            isAttackPowerupActive = true;
            Destroy(other.gameObject);
            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
                isGreenPowerupActive = false; // set other powerups inactive
                hasPowerup = false;
                powerupIndicator.gameObject.SetActive(false);
                powerupIndicatorGreen.gameObject.SetActive(false);

            }

            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
            powerupIndicatorAttack.gameObject.SetActive(true);
        }
    }

    IEnumerator FreezeXYPositions()
    {
        playerRb.angularVelocity = Vector3.zero;

        // freeze the X and Z positions of player

        playerRb.constraints = RigidbodyConstraints.FreezePositionX;
        playerRb.constraints = RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(1.5f);

        playerRb.constraints = RigidbodyConstraints.None;
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        isGreenPowerupActive = false;
        isAttackPowerupActive = false;
        powerupIndicator.gameObject.SetActive(false);
        powerupIndicatorGreen.gameObject.SetActive(false);
        powerupIndicatorAttack.gameObject.SetActive(false);
        StopCoroutine(SmashAttack());


    }

    public void SpawnProjectile()
    {
        float[] targetAngles = { 20f, -20f, 60f, -60f };

        // Get the player's position
        Vector3 spawnPosition = transform.position;

        // Spawn four projectiles with the specified angles
        for (int i = 0; i < targetAngles.Length; i++)
        {
            float angle = targetAngles[i];
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            Vector3 spawnOffset = rotation * Vector3.forward * projectileSpawnDistance;
            // Instantiate the projectile and set its position and angle
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position + spawnOffset, rotation);

            // You can set additional properties for the projectile here if needed
            Projectile projectileScript = newProjectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.angle = angle;
            }
        }
    }


    // Attack powerup
    private IEnumerator SmashAttack()
    {
        float smashSpeed = speed * 15;
        isMovingToTarget = true;
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.up * 4.0f; // Set the target 5 units above the original position


        float journeyLength = Vector3.Distance(transform.position, targetPosition);
        float startTime = Time.time;

        while (Time.time - startTime < journeyLength / smashSpeed)
        {
            float distanceCovered = (Time.time - startTime) * smashSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(originalPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        // Ensure that the object reaches the target position exactly
        transform.position = targetPosition;

        isMovingToTarget = false;
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
        if (collision.gameObject.CompareTag("floor"))
        {
            playerRb.constraints = RigidbodyConstraints.None;

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);


            if (collision.gameObject.CompareTag("floor") && jumped)
            {
                // Get all colliders within the explosion radius
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

                // Apply force to enemies found within the explosion radius
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        Rigidbody enemyRigidbody = hitCollider.GetComponent<Rigidbody>();
                        if (enemyRigidbody != null)
                        {
                            Vector3 direction = hitCollider.transform.position - transform.position;
                            enemyRigidbody.AddForce(direction.normalized * power, ForceMode.VelocityChange);
                        }
                    }
                }

                isGrounded = true;
                jumped = false;
            }
        }
    }
}


