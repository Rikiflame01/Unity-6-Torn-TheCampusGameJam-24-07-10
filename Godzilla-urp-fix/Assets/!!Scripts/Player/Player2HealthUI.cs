using UnityEngine;
using UnityEngine.UI;

public class Player2HealthUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        // Subscribe to the event
        EventsManager.Instance.OnP2HealthChanged.AddListener(UpdateHealthUI);
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        EventsManager.Instance.OnP2HealthChanged.RemoveListener(UpdateHealthUI);
    }

    /// <summary>
    /// Updates the health slider when the event is triggered.
    /// </summary>
    /// <param name="currentHealth">The current health of Player 2.</param>
    /// <param name="maxHealth">The maximum health of Player 2.</param>
    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
