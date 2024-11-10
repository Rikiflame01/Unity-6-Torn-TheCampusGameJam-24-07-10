using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider healthSlider;
    private IHealth health;

    private void Awake()
    {
        health = GetComponent<IHealth>();

        if (health == null)
        {
            Debug.LogError("No IHealth component found on " + gameObject.name);
        }

        if (healthSlider == null)
        {
            Debug.LogError("No Slider component assigned to " + gameObject.name);
        }
    }

    private void Start()
    {
        if (health != null && healthSlider != null)
        {
            healthSlider.maxValue = health.GetMaxHealth();
            healthSlider.value = health.GetCurrentHealth();
        }

        Health healthComponent = health as Health;
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged.AddListener(UpdateHealthSlider);
        }
    }

    private void UpdateHealthSlider(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void OnDestroy()
    {
        Health healthComponent = health as Health;
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged.RemoveListener(UpdateHealthSlider);
        }
    }
}
