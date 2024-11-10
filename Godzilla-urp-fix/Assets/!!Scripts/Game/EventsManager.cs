using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }

    [System.Serializable]
    public class HealthChangedEvent : UnityEvent<int, int> { }

    [System.Serializable]
    public class ShakeEvent : UnityEvent<string> { }

    [System.Serializable]
    public class GameEvent : UnityEvent<string> { }

    public UnityAction<string, string> OnDiedEvent;

    public UnityEvent<string> OnTailAttackCooldown = new UnityEvent<string>();
    public UnityEvent<string> OnHeadbuttCooldown = new UnityEvent<string>();
    public UnityEvent<string> OnChargeAttackCooldown = new UnityEvent<string>();
    public UnityEvent<string> OnAerialAttackCooldown = new UnityEvent<string>();
    public UnityEvent<string> OnDashAttackCooldown = new UnityEvent<string>();


    public ShakeEvent OnShakeEvent;
    public GameEvent OnChargeAttack;
    public GameEvent OnTailAttack;
    public GameEvent OnHeadbutt;
    public GameEvent OnAerialAttack;
    public GameEvent OnDashAttack;
    public GameEvent OnPlayerHit;
    public GameEvent OnPlayerDead;
    public GameEvent OnWinEvent;
    public GameEvent OnPlayerWarning;
    public GameEvent OnPlayerTriggerJetWarning;
    public GameEvent OnPlayerTriggerJetBombDrop;

    public HealthChangedEvent OnHealthChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerTailAttackCooldown()
    {
        OnTailAttackCooldown.Invoke("TailAttack");
    }

    public void TriggerHeadbuttCooldown()
    {
        OnHeadbuttCooldown.Invoke("Headbutt");
    }

    public void TriggerChargeAttackCooldown()
    {
        OnChargeAttackCooldown.Invoke("ChargeAttack");
    }

    public void TriggerAerialAttackCooldown()
    {
        OnAerialAttackCooldown.Invoke("AerialAttack");
    }

    public void TriggerDashAttackCooldown()
    {
        OnDashAttackCooldown.Invoke("DashAttack");
    }

    public void TriggerShakeEvent(string shakeType)
    {
        OnShakeEvent?.Invoke(shakeType);
    }

    public void TriggerChargeAttack()
    {
        OnChargeAttack?.Invoke("ChargeAttack");
    }

    public void TriggerTailAttack()
    {
        OnTailAttack?.Invoke("TailAttack");
    }

    public void TriggerHeadbutt()
    {
        OnHeadbutt?.Invoke("Headbutt");
    }

    public void TriggerPlayerHit()
    {
        OnPlayerHit?.Invoke("PlayerHit");
    }

    public void TriggerPlayerDead()
    {
        OnPlayerDead?.Invoke("PlayerDead");
    }

    public void TriggerPlayerWin()
    {
        OnWinEvent?.Invoke("PlayerWin");
    }

    public void TriggerPlayerHealthChanged(int currentHealth, int maxHealth)
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public void TriggerOnDiedEvent(string objectId, string objectTag)
    {
        OnDiedEvent?.Invoke(objectId, objectTag);
    }
    public void TriggerPlayerWarning()
    {
        OnPlayerWarning?.Invoke("PlayerWarning");
    }
    public void TriggerPlayerTriggerJetWarning()
    {
        OnPlayerTriggerJetWarning?.Invoke("PlayerTriggerJetWarning");
    }
    public void TriggerPlayerTriggerJetBombDrop()
    {
        OnPlayerTriggerJetBombDrop?.Invoke("PlayerTriggerJetBombDrop");
    }
}
