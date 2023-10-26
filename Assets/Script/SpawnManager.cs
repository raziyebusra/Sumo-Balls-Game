using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemies;

    public GameObject powerupPrefab;
    private float spawnRange = 9.0f;

    public int enemyCount;
    public int waveNumber = 0;

    void Start()
    {

        SpawnEnemyWave(waveNumber);

    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 1)
        {
            waveNumber++;
            SpawnEnemyWave(1);
        }

    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, enemies.Length);

            Instantiate(enemies[randomIndex], GenerateSpawnPosition(), enemies[randomIndex].transform.rotation);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);

        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
