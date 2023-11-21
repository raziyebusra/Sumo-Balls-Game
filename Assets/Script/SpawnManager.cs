using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemies;

    public GameObject[] powerupPrefab;
    public GameObject[] bossPrefab;
    private float spawnRange = 9.0f;

    public int enemyCount;
    public int bossCount;

    //    bool isBossSpawned = false;
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
        GameObject[] bossInTheScene = GameObject.FindGameObjectsWithTag("Boss");


        bossCount = bossInTheScene.Length;
        if ((waveNumber % 3 == 0) && (bossCount < 1)) { SpawnBoss(); }

    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, enemies.Length);
            int powerupRandomIndex = Random.Range(0, powerupPrefab.Length);

            Instantiate(enemies[randomIndex], GenerateSpawnPosition(), enemies[randomIndex].transform.rotation);

            Instantiate(powerupPrefab[powerupRandomIndex], GenerateSpawnPosition(), powerupPrefab[powerupRandomIndex].transform.rotation);

        }
    }
    void SpawnBoss()
    {


        Instantiate(bossPrefab[0], GenerateSpawnPosition(), bossPrefab[0].transform.rotation);

        //  isBossSpawned = true;

    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
