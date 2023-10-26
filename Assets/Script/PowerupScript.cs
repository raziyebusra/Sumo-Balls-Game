using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public int numberOfProjectiles = 4;  // Number of projectiles to spawn

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player collided with the power-up
        {
            // Apply the power-up effect to the player here
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ActivatePowerUp(projectilePrefab, numberOfProjectiles);
            }

            // Destroy the power-up
            Destroy(gameObject);
        }
    }
}
