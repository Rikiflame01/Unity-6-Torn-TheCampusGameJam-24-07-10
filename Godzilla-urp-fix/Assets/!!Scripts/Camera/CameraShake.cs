using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform player;
    public float smallShakeIntensity = 0.1f;
    public float mediumShakeIntensity = 0.3f;
    public float largeShakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;
    public float maxXOffset = 0.5f;

    private Vector3 originalOffset;

    private void Start()
    {
        originalOffset = transform.position - player.position;


        EventsManager.Instance.OnShakeEvent.AddListener(HandleShakeEvent);
    }

    private void OnDestroy()
    {
        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.OnShakeEvent.RemoveListener(HandleShakeEvent);
        }
    }

    private void HandleShakeEvent(string shakeType)
    {
        switch (shakeType)
        {
            case "small":
                ShakeCamera(smallShakeIntensity);
                break;
            case "medium":
                ShakeCamera(mediumShakeIntensity);
                break;
            case "large":
                ShakeCamera(largeShakeIntensity);
                break;
            default:
                Debug.LogWarning("Unknown shake type: " + shakeType);
                break;
        }
    }

    public void ShakeCamera(float intensity)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(intensity, shakeDuration));
    }

    private IEnumerator Shake(float intensity, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Mathf.Clamp(Random.Range(-1f, 1f) * intensity, -maxXOffset, maxXOffset);
            float y = Random.Range(-1f, 1f) * intensity;

            transform.position = player.position + originalOffset + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = player.position + originalOffset;
    }
}
