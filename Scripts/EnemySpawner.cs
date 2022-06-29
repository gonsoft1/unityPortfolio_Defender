using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] createPosition;
    public float spawnTime;
    float lastSpawnTime = 0;

   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EnemyCreate();
        }

        if(Time.time >= lastSpawnTime + spawnTime)
        {
            lastSpawnTime = Time.time;
            EnemyCreate();
        }
    }

    void EnemyCreate()
    {
        GameObject enemy = Instantiate(enemyPrefab, createPosition[Random.Range(0, 8)].position, Quaternion.identity) as GameObject;
        GameManager.Instance.enemyCount.Add(enemy);
    }

}
