using UnityEngine;

public class AerialAttackHandler : MonoBehaviour
{
    public GameObject shockwavePrefab;
    public float attackRadius = 50f;
    public float cooldownDuration = 5f;
    public float chargeTime = 2f;

    [HideInInspector]
    public bool isOnCooldown = false;

    private float cooldownTimer = 0f;
    private bool isCharging = false;
    private float currentChargeTime = 0f;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }

        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            if (currentChargeTime >= chargeTime)
            {
                ExecuteAerialAttack();
                isCharging = false;
                currentChargeTime = 0f;
            }
        }
    }

    public void StartCharging()
    {
        isCharging = true;
        currentChargeTime = 0f;
    }

    public void CancelCharging()
    {
        isCharging = false;
        currentChargeTime = 0f;
    }

    public void ExecuteAerialAttack()
    {
        SFXManager.Instance.PlayT_RexAudio("thud");
        EventsManager.Instance.TriggerShakeEvent("small");
        Instantiate(shockwavePrefab, transform.position, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "HelicopterEnemy1")
            {
                hitCollider.GetComponentInParent<HelicopterController>().DisableMovementForSeconds(5f);
            }
        }

        isOnCooldown = true;
        EventsManager.Instance.TriggerAerialAttackCooldown();
    }

}
