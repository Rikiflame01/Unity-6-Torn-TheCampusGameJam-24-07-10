using UnityEngine;
using System.Collections;

public class TokyoTower : MonoBehaviour
{
    public float checkInterval = 0.5f;
    public float detectionRadius = 10.0f;
    private GameObject player;
    private IHealth healthComponent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthComponent = GetComponent<IHealth>();
        StartCoroutine(CheckHealthRoutine());
    }

    private IEnumerator CheckHealthRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            if (player != null && IsPlayerNearby())
            {
                CheckHealth();
            }
        }
    }

    private bool IsPlayerNearby()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= detectionRadius;
    }

    private void CheckHealth()
    {
        if (healthComponent != null && healthComponent.GetCurrentHealth() <= 0)
        {
            EventsManager.Instance.TriggerPlayerWin();
        }
    }
}
