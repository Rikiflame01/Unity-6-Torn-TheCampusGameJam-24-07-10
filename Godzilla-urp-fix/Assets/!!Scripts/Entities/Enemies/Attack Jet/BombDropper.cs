using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JetBombDropper : MonoBehaviour
{
    public GameObject[] bombs;
    public float dropSpeed = 5f;

    private void OnEnable()
    {
        EventsManager.Instance.OnPlayerTriggerJetBombDrop.AddListener(DropBombs);
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnPlayerTriggerJetBombDrop.RemoveListener(DropBombs);
    }

    private IEnumerator BombDelay()
    {
        yield return new WaitForSeconds(2.5f);
        EventsManager.Instance.OnShakeEvent?.Invoke("small");
        int useCount = GameManager.Instance.jetBombMechanicUseCount;

        int bombsToDrop = useCount == 1 ? 2 : 3;

        GameObject[] rocketWarnings = GameObject.FindGameObjectsWithTag("RocketWarning");

        if (rocketWarnings.Length == 0)
        {
            Debug.LogError("No RocketWarning objects found in the scene.");
            yield break;
        }

        List<Transform> targets = new List<Transform>();
        foreach (GameObject rocketWarning in rocketWarnings)
        {
            foreach (Transform child in rocketWarning.transform)
            {
                if (child.CompareTag("JetBombTarget"))
                {
                    targets.Add(child);
                }
            }
        }

        if (targets.Count < bombsToDrop)
        {
            Debug.LogError("Not enough JetBombTarget objects in the scene.");
            yield break;
        }

        ShuffleList(targets);

        List<GameObject> availableBombs = new List<GameObject>(bombs);

        for (int i = 0; i < bombsToDrop; i++)
        {
            if (availableBombs.Count > 0)
            {
                GameObject bomb = availableBombs[0];
                availableBombs.RemoveAt(0);
                bomb.transform.SetParent(null);
                EnableMeshRenderers(bomb);
                PointTowardsTarget(bomb, targets[i]);
                StartCoroutine(MoveBombToTarget(bomb, targets[i]));
                Destroy(bomb, 1f);
            }
        }

        bombs = availableBombs.ToArray();

        GameManager.Instance.jetBombMechanicUseCount++;
    }

    private void DropBombs(string eventType)
    {
        StartCoroutine(BombDelay());
    }

    private IEnumerator MoveBombToTarget(GameObject bomb, Transform target)
    {
        while (bomb != null && target != null && Vector3.Distance(bomb.transform.position, target.position) > 0.1f)
        {
            if (bomb == null || target == null)
            {
                yield break;
            }

            bomb.transform.position = Vector3.MoveTowards(bomb.transform.position, target.position, dropSpeed * Time.deltaTime);
            yield return null;
        }

        if (bomb != null && target != null)
        {
            // Bomb reached the target, handle impact or explosion here
        }
    }

    private void EnableMeshRenderers(GameObject obj)
    {
        foreach (MeshRenderer meshRenderer in obj.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
    }

    private void PointTowardsTarget(GameObject obj, Transform target)
    {
        Vector3 direction = (target.position - obj.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        obj.transform.rotation = lookRotation;
    }

    private void ShuffleList(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
