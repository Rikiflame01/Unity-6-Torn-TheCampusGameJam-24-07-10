using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;          
    public float fadeDuration = 0.5f;  

    private Coroutine fadeCoroutine;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float targetValue = (float)currentHealth / maxHealth;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(SmoothDecrease(targetValue));
    }

    private IEnumerator SmoothDecrease(float targetValue)
    {
        float initialSliderValue = healthSlider.value;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            healthSlider.value = Mathf.Lerp(initialSliderValue, targetValue, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthSlider.value = targetValue;
    }
}
