using UnityEngine;
using System.Collections;

public class PlayerWarning : MonoBehaviour
{
    public float checkInterval = 2f;
    public Color startColor = Color.white;
    public Color endColor = Color.gray;
    public float lerpDuration = 1f;
    public int flashCount = 3;
    public float flashDuration = 0.1f;
    public GameObject vfxPrefab;
    public Vector3 vfxScale = Vector3.one;
    public float vfxYOffset = 0f;

    private bool isPlayerInside = false;
    private Renderer objectRenderer;
    private float lerpTimer;
    private bool isLerpingForward = true;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component is missing.");
            return;
        }

        objectRenderer.material.color = startColor;
        StartCoroutine(CheckPlayerPresence());
    }

    private void Update()
    {
        HandleBreathingEffect();
    }

    private void HandleBreathingEffect()
    {
        lerpTimer += Time.deltaTime;
        float normalizedTime = lerpTimer / lerpDuration;

        if (isLerpingForward)
        {
            objectRenderer.material.color = Color.Lerp(startColor, endColor, normalizedTime);
        }
        else
        {
            objectRenderer.material.color = Color.Lerp(endColor, startColor, normalizedTime);
        }

        if (normalizedTime >= 1f)
        {
            lerpTimer = 0f;
            isLerpingForward = !isLerpingForward;
        }
    }

    private IEnumerator CheckPlayerPresence()
    {
        yield return new WaitForSeconds(checkInterval);

        StartCoroutine(FlashEffect());
    }

    private void ApplyDamage()
    {

        GameObject playerObject = GameObject.FindWithTag("Player");

        IHealth health = playerObject.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(30);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {
            yield return StartCoroutine(LerpOpacity(0f, 1f, flashDuration));
            yield return StartCoroutine(LerpOpacity(1f, 0f, flashDuration));
        }
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("Collider component is missing.");
            yield return null;
        }

        collider.isTrigger = true;
        bool playerDetected = isPlayerInside;
        Debug.Log($"Player detected: {playerDetected}");

        if (playerDetected == true)
        {
            if (vfxPrefab != null)
            {
                Vector3 vfxPosition = transform.position + new Vector3(0, vfxYOffset, 0);
                GameObject vfxInstance = Instantiate(vfxPrefab, vfxPosition, Quaternion.identity);
                vfxInstance.transform.localScale = vfxScale;
            }

            SFXManager.Instance.PlayAudioWithVolume("explosion",4);
            ApplyDamage();
        }
        else {
            if (vfxPrefab != null)
            {
                Vector3 vfxPosition = transform.position + new Vector3(0, vfxYOffset, 0);
                GameObject vfxInstance = Instantiate(vfxPrefab, vfxPosition, Quaternion.identity);
                vfxInstance.transform.localScale = vfxScale;
            }
            SFXManager.Instance.PlayAudioWithVolume("explosion", 4);
            gameObject.SetActive(false); }


    }

    private IEnumerator LerpOpacity(float startOpacity, float endOpacity, float duration)
    {
        float timer = 0f;
        Color color = objectRenderer.material.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startOpacity, endOpacity, timer / duration);
            objectRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
