using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CooldownManager : MonoBehaviour
{
    [System.Serializable]
    public class AbilityCooldown
    {
        public string abilityName;
        public Image cooldownImage;
        public float cooldownDuration;
        [HideInInspector] public float cooldownTimer;
        [HideInInspector] public bool isOnCooldown;
    }

    public List<AbilityCooldown> abilities = new List<AbilityCooldown>();

    private void Start()
    {
        foreach (var ability in abilities)
        {
            if (ability.cooldownImage != null)
            {
                ability.cooldownImage.fillAmount = 1f;
            }
            ability.isOnCooldown = false;
            ability.cooldownTimer = 0f;
        }

        EventsManager.Instance.OnTailAttackCooldown.AddListener(OnTailAttackCooldown);
        EventsManager.Instance.OnHeadbuttCooldown.AddListener(OnHeadbuttCooldown);
        EventsManager.Instance.OnChargeAttackCooldown.AddListener(OnChargeAttackCooldown);
        EventsManager.Instance.OnAerialAttackCooldown.AddListener(OnAerialAttackCooldown);
        EventsManager.Instance.OnDashAttackCooldown.AddListener(OnDashAttackCooldown);
    }

    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (ability.isOnCooldown)
            {
                ability.cooldownTimer += Time.deltaTime;
                float ratio = ability.cooldownTimer / ability.cooldownDuration;

                if (ability.cooldownImage != null)
                {
                    ability.cooldownImage.fillAmount = ratio;
                }

                if (ability.cooldownTimer >= ability.cooldownDuration)
                {
                    ability.isOnCooldown = false;
                    ability.cooldownTimer = 0f;
                    if (ability.cooldownImage != null)
                    {
                        ability.cooldownImage.fillAmount = 1f;
                    }
                }
            }
        }
    }

    private void OnTailAttackCooldown(string eventName)
    {
        StartCooldown("TailAttack");
    }

    private void OnHeadbuttCooldown(string eventName)
    {
        StartCooldown("Headbutt");
    }

    private void OnChargeAttackCooldown(string eventName)
    {
        StartCooldown("ChargeAttack");
    }

    private void OnAerialAttackCooldown(string eventName)
    {
        StartCooldown("AerialAttack");
    }

    private void OnDashAttackCooldown(string eventName)
    {
        StartCooldown("DashAttack");
    }

    private void StartCooldown(string abilityName)
    {
        var ability = abilities.Find(a => a.abilityName == abilityName);
        if (ability != null)
        {
            ability.isOnCooldown = true;
            ability.cooldownTimer = 0f;
            if (ability.cooldownImage != null)
            {
                ability.cooldownImage.fillAmount = 0f;
            }
        }
    }
}
