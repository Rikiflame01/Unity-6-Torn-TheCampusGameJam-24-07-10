using UnityEngine;
using UnityEditor;
using System.Collections;

public class JetHandler : MonoBehaviour
{
    public Transform player;
    public float xOffset = 5f;
    public Transform startPoint;
    public Transform endPoint;
    public float travelSpeed = 5f;

    private bool isTraveling = false;

    private void OnEnable()
    {
        EventsManager.Instance.OnPlayerWarning.AddListener(OnPlayerWarningTriggered);
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnPlayerWarning.RemoveListener(OnPlayerWarningTriggered);
    }

    private void Update()
    {
        if (!isTraveling)
        {
            UpdatePositionWithOffset();
        }
        else
        {
            TravelToPoint();
        }
    }

    private void UpdatePositionWithOffset()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned.");
            return;
        }

        Vector3 newPosition = new Vector3(player.position.x + xOffset, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    private void TravelToPoint()
    {
        float step = travelSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);

        if (Vector3.Distance(transform.position, endPoint.position) < 0.001f)
        {
            isTraveling = false;
            DisableMeshRenderers();
        }
    }


    public void DisableMeshRenderers()
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnPlayerWarningTriggered(string eventType)
    {
        StartCoroutine(JetTravel());
    }

    private IEnumerator JetTravel()
    {
        yield return new WaitForSeconds(2.5f);
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
        if (startPoint != null && endPoint != null)
        {
            transform.position = startPoint.position;
            isTraveling = true;
        }
        else
        {
            Debug.LogError("StartPoint or EndPoint is not assigned.");
        }
    }
}
