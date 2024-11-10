using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public float mirrorAngle = 10f;         
    public float scaleAmount = 1.1f;        
    public float animationDuration = 0.3f;  
    public float interval = 1f;             

    public float shakeDuration = 0.2f;       
    public float shakeMagnitude = 5f;        

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private float elapsedTime;
    private bool animating;
    private bool isShaking;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogWarning("AttentionGrabber script requires a RectTransform component.");
            return;
        }
        originalScale = rectTransform.localScale;
        elapsedTime = interval;
        animating = false;
        isShaking = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Period) && !isShaking)
        {
            StartCoroutine(ShakeAnimation());
        }

        if (!isShaking)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0 && !animating)
            {
                animating = true;
                StartCoroutine(AnimateAttention());
                elapsedTime = interval;
            }
        }
    }

    private System.Collections.IEnumerator AnimateAttention()
    {
        float halfDuration = animationDuration / 2f;

        for (float t = 0; t < 1; t += Time.deltaTime / halfDuration)
        {
            float angle = Mathf.SmoothStep(0, -mirrorAngle, t);
            float scale = Mathf.SmoothStep(originalScale.x, scaleAmount, t);
            rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            rectTransform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        for (float t = 0; t < 1; t += Time.deltaTime / halfDuration)
        {
            float angle = Mathf.SmoothStep(-mirrorAngle, mirrorAngle, t);
            float scale = Mathf.SmoothStep(scaleAmount, originalScale.x, t);
            rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            rectTransform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        for (float t = 0; t < 1; t += Time.deltaTime / halfDuration)
        {
            float angle = Mathf.SmoothStep(mirrorAngle, 0, t);
            float scale = Mathf.SmoothStep(originalScale.x, originalScale.x, t);
            rectTransform.localEulerAngles = new Vector3(0, 0, angle);
            rectTransform.localScale = originalScale;
            yield return null;
        }

        animating = false;
    }

    private System.Collections.IEnumerator ShakeAnimation()
    {
        isShaking = true;
        Vector3 originalPosition = rectTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            rectTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = originalPosition;
        isShaking = false;
    }
}
