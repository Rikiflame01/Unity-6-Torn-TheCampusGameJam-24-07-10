using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public SpawnManager _spawnManager;
    public GameObject[] buildings;
    public GameObject[] spawnPositions = new GameObject[3];
    public LayerMask groundLayer;
    public LayerMask ignoreLayers;
    public float spawnCheckRadius = 4f;
    public float positionCheckInterval = 3f;

    private Dictionary<Vector3, float> occupiedPositions = new Dictionary<Vector3, float>();

    void Start()
    {
        _spawnManager = gameObject.GetComponent<SpawnManager>();
        SpawnAllBuildings();
        StartCoroutine(SpawnObject());
    }

    void SpawnAllBuildings()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Vector3 spawnPosition = spawnPositions[i].transform.position;

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                spawnPosition = hit.point;
            }
            else
            {
                Debug.LogWarning("Ground not found at spawn position. Using original spawn position.");
            }

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, spawnCheckRadius, ~ignoreLayers);
            bool isNearObject = false;
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    isNearObject = true;
                    break;
                }
            }

            if (!isNearObject)
            {
                int buildingIndex = i % buildings.Length;
                GameObject newObject = Instantiate(buildings[buildingIndex], spawnPosition, Quaternion.identity);

                //Adjust the building's position based on its collider
                Collider buildingCollider = newObject.GetComponent<Collider>();
                if (buildingCollider != null)
                {
                    Vector3 colliderBottom = buildingCollider.bounds.min;
                    float adjustment = spawnPosition.y - colliderBottom.y;
                    newObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + adjustment, spawnPosition.z);
                }

                _spawnManager.spawnedBuildings.Add(newObject);
                occupiedPositions[spawnPosition] = Time.time;
            }
        }
    }

    public IEnumerator SpawnObject()
    {
        while (true)
        {
            int objToSpawn = Random.Range(0, buildings.Length);
            int spawnRate = Random.Range(5, 10);
            int spawnPos = Random.Range(0, spawnPositions.Length);

            Vector3 spawnPosition = spawnPositions[spawnPos].transform.position;

            //Check if the spawn position is already occupied and adjust if necessary
            List<Vector3> keysToRemove = new List<Vector3>();
            foreach (KeyValuePair<Vector3, float> entry in occupiedPositions)
            {
                if (Time.time - entry.Value > positionCheckInterval)
                {
                    keysToRemove.Add(entry.Key);
                }
                if (Vector3.Distance(spawnPosition, entry.Key) < spawnCheckRadius)
                {
                    spawnPosition.x += 5;
                }
            }

            foreach (var key in keysToRemove)
            {
                occupiedPositions.Remove(key);
            }

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                spawnPosition = hit.point;
            }
            else
            {
                Debug.LogWarning("Ground not found at spawn position. Using original spawn position.");
            }

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, spawnCheckRadius, ~ignoreLayers);
            bool isNearObject = false;
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    isNearObject = true;
                    break;
                }
            }

            if (!isNearObject)
            {
                GameObject newObject = Instantiate(buildings[objToSpawn], spawnPosition, Quaternion.identity);

                Collider buildingCollider = newObject.GetComponent<Collider>();
                if (buildingCollider != null)
                {
                    Vector3 colliderBottom = buildingCollider.bounds.min;
                    float adjustment = spawnPosition.y - colliderBottom.y;
                    newObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + adjustment, spawnPosition.z);
                }

                _spawnManager.spawnedBuildings.Add(newObject);
                occupiedPositions[spawnPosition] = Time.time;
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

}
