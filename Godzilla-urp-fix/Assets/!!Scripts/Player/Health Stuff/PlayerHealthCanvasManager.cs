using UnityEngine;

public class PlayerHealthCanvasManager : MonoBehaviour
{
    public HealthBar healthBar;
    private IHealth playerHealth;
    public HurtSpriteHandler hurtSpriteHandler;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("IHealth component not found on player.");
            return;
        }

        if (hurtSpriteHandler == null)
        {
            Debug.LogError("HurtSpriteHandler component not found on player.");
            return;
        }

        EventsManager.Instance.OnHealthChanged.AddListener(UpdateHealthBar);
        UpdateHealthBar(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        float healthPercentage = (float)currentHealth / maxHealth;
        hurtSpriteHandler.UpdateHealthState(healthPercentage);

        hurtSpriteHandler.TriggerHurtSprite();
    }
}
