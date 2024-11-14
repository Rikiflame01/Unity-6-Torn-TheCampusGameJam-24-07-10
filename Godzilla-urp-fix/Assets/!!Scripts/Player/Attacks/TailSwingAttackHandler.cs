using UnityEngine;

public class TailSwingAttackHandler : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject tailSwingColliderObject;
    public float tailSwingCooldown = 1.5f;
    public Vector3 offsetRight;
    public Vector3 offsetLeft;

    private bool isOnCooldown;
    private float cooldownTimer;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        if (tailSwingColliderObject != null)
        {
            tailSwingColliderObject.SetActive(false);
        }
        isOnCooldown = false;
        cooldownTimer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventsManager.Instance.OnTailAttack.AddListener(ActivateTailSwing);
        }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= tailSwingCooldown)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    void ActivateTailSwing(GameObject gameObject)
    {
        if (gameObject != playerObject)
        {
            return;
        }
        if (!isOnCooldown && tailSwingColliderObject != null)
        {
            PositionTailSwingCollider();
            tailSwingColliderObject.SetActive(true);
            isOnCooldown = true;
            EventsManager.Instance.TriggerTailAttackCooldown();
            Invoke("DeactivateTailSwing", 0.5f);
        }
    }

    void PositionTailSwingCollider()
    {
        if (spriteRenderer.flipX)
        {
            tailSwingColliderObject.transform.localPosition = offsetLeft;
        }
        else
        {
            tailSwingColliderObject.transform.localPosition = offsetRight;
        }
    }

    void DeactivateTailSwing()
    {
        if (tailSwingColliderObject != null)
        {
            tailSwingColliderObject.SetActive(false);
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
