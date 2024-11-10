using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreTextShaker : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private float shakeDuration = 1f;
    private float shakeMagnitude = 10f;
    private float colorChangeSpeed = 1f;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        if (scoreText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
        }
        originalPosition = scoreText.rectTransform.localPosition;
    }

    public void ShakeText()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeAndChangeColor());
        }
    }

    private IEnumerator ShakeAndChangeColor()
    {
        isShaking = true;
        float elapsed = 0.0f;
        float hue = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            scoreText.rectTransform.localPosition = originalPosition + new Vector3(x, y, 0);

            hue += Time.deltaTime * colorChangeSpeed;
            if (hue > 1.0f)
            {
                hue -= 1.0f;
            }
            scoreText.color = Color.HSVToRGB(hue, 1.0f, 1.0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        scoreText.rectTransform.localPosition = originalPosition;
        scoreText.color = Color.white;
        isShaking = false;
    }
}
