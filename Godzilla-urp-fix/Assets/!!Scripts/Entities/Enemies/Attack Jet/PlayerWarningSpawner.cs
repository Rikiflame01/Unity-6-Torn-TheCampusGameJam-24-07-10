using UnityEngine;
using System.Collections.Generic;

public class PlayerWarningSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public Transform player;

    public float distanceInFront = 5f;
    public float distanceBehind = 5f;
    public float distanceToLeft = 5f;
    public float distanceToRight = 5f;

    private void OnEnable()
    {
        EventsManager.Instance.OnPlayerTriggerJetWarning.AddListener(SpawnPrefabs);
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnPlayerTriggerJetWarning.RemoveListener(SpawnPrefabs);
    }

    private void SpawnPrefabs(string eventType)
    {
        if (GameManager.Instance.jetBombMechanicUseCount == 3)
        {
            return;
        }

        if (prefabToSpawn == null || player == null)
        {
            Debug.LogError("Prefab or player transform is not assigned.");
            return;
        }

        EventsManager.Instance.OnPlayerWarning?.Invoke("PlayerWarning");

        List<Vector3> positions = new List<Vector3>
        {
            player.position + player.forward * distanceInFront,
            player.position - player.forward * distanceBehind,
            player.position - player.right * distanceToLeft,
            player.position + player.right * distanceToRight
        };

        int useCount = GameManager.Instance.jetBombMechanicUseCount;

        GameManager.Instance.jetBombMechanicUseCount++;

        int positionsToActivate = useCount == 0 ? 2 : 3;

        ShuffleList(positions);

        for (int i = 0; i < positionsToActivate; i++)
        {
            Instantiate(prefabToSpawn, positions[i], Quaternion.identity);
        }
        EventsManager.Instance.TriggerPlayerTriggerJetBombDrop();
    }

    private void ShuffleList(List<Vector3> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
