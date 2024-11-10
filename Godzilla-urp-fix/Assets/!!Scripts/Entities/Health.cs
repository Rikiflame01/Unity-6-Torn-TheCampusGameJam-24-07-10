using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealth
{
    void TakeDamage(int amount);
    void Heal(int amount);
    int GetCurrentHealth();
    int GetMaxHealth();
}

public class Health : MonoBehaviour, IHealth
{
    [SerializeField, Tooltip("The maximum health value for this object.")]
    private int maxHealth = 100;

    private int currentHealth;
    private string objectId;

    [System.Serializable]
    public class HealthChangedEvent : UnityEvent<int> { }

    [System.Serializable]
    public class DeathEvent : UnityEvent { }

    public HealthChangedEvent OnHealthChanged;
    public DeathEvent OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth);
        objectId = gameObject.GetInstanceID().ToString();
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged.Invoke(currentHealth);

        if (this.gameObject.CompareTag("Player"))
        {
            EventsManager.Instance.TriggerPlayerHit();
            EventsManager.Instance.TriggerPlayerHealthChanged(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                EventsManager.Instance.TriggerPlayerDead();
            }
        }

        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                OnDied.Invoke();
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged.Invoke(currentHealth);

        if (this.gameObject.CompareTag("Player"))
        {
            EventsManager.Instance.TriggerPlayerHealthChanged(currentHealth, maxHealth);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private bool isDead = false;

    private void Die()
    {
        if (this.gameObject.CompareTag("TankEnemy1"))
        {
            Invoke("HandleTankDeath", 5);
        }
        if (this.gameObject.CompareTag("HelicopterEnemy1"))
        {
            EventsManager.Instance.TriggerShakeEvent("Medium");
            Destroy(this.gameObject);       
        }
        if (this.gameObject.CompareTag("JetEnemy1"))
        {
            EventsManager.Instance.TriggerShakeEvent("Large");
            Destroy(this.gameObject);
        }
        EventsManager.Instance.TriggerOnDiedEvent(objectId, gameObject.tag);
    }

    public void HandleTankDeath()
    {
        EventsManager.Instance.TriggerShakeEvent("small");
        this.gameObject.SetActive(false);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
