using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public SpawnManager _spawnManager;
    public GameObject[] enemies;

    public GameObject player;

    public GameObject[] spawnPositions = new GameObject[3];

    public Vector3 spawnPosition;

    //public static GameObject[] spawnedObjects = new GameObject[10] ;
    void Start()
    {
        _spawnManager = gameObject.GetComponent<SpawnManager>();
        StartCoroutine(SpawnObject());
    }

    public IEnumerator SpawnObject()
    {
        int i = 1;
        while (true)
        {

            int objToSpwn = Random.Range(0, enemies.Length);
            int spawnRate = Random.Range(3, 10);
            int spawnPos = Random.Range(0, 4);

            spawnPosition = spawnPositions[spawnPos].transform.position;
            for (int j = 0; j < _spawnManager.spawnedBuildings.Count; j++)
            {
                if (spawnPosition == _spawnManager.spawnedBuildings[i].transform.position)
                {
                    spawnPosition.x = spawnPosition.x + 5;
                }
            }
            /* //Generates apropriate spawn position based on randomly selected lane and object prefab
             if (spawnPos == 0 && objToSpwn == 0 || spawnPos == 0 && objToSpwn == 2)
             {
                 spawnPosition = spawnPositions[0].transform.position;
             }
             else if (spawnPos == 1 && objToSpwn == 0 || spawnPos == 1 && objToSpwn == 2)
             {
                 spawnPosition = spawnPositions[1].transform.position;
             }
             else if (spawnPos == 2 && objToSpwn == 0 || spawnPos == 2 && objToSpwn == 2)
             {
                 spawnPosition = spawnPositions[0].transform.position;
             }

             else
             {
                 spawnPosition = spawnPositions[1].transform.position;
             }*/

            GameObject newObject = Instantiate(enemies[objToSpwn], spawnPosition, Quaternion.identity);
            _spawnManager.spawnedEnemies.Add(newObject);

            i++;
            yield return new WaitForSeconds(spawnRate);

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
