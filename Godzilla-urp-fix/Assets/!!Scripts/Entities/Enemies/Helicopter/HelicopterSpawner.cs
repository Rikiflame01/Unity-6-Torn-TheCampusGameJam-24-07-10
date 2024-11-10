using UnityEngine;

public class HelicopterSpawner : MonoBehaviour
{
    public GameObject helicopterPrefab;
    public Transform spawnTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnHelicopter();
        }
    }

    private void SpawnHelicopter()
    {
        if (helicopterPrefab != null && spawnTransform != null)
        {
            Instantiate(helicopterPrefab, spawnTransform.position, spawnTransform.rotation);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Helicopter Prefab or Spawn Transform is not assigned.");
        }
    }
}
