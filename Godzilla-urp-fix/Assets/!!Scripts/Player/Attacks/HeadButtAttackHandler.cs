using UnityEngine;

public class HeadbuttAttackHandler : MonoBehaviour
{
    public GameObject headbuttColliderObject;
    public float headbuttCooldown = 1.5f;
    public Vector3 offsetRight;
    public Vector3 offsetLeft;

    public bool isOnCooldown;
    private float cooldownTimer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (headbuttColliderObject != null)
        {
            headbuttColliderObject.SetActive(false);
        }
        isOnCooldown = false;
        cooldownTimer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventsManager.Instance.OnHeadbutt.AddListener(ActivateHeadbutt);
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= headbuttCooldown)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    void ActivateHeadbutt(string eventName)
    {
        if (!isOnCooldown && headbuttColliderObject != null)
        {
            isOnCooldown = true;
            EventsManager.Instance.TriggerHeadbuttCooldown();

            PositionHeadbuttCollider();
            Invoke("ActivateHeadbuttCollider", 0.1f);
            Invoke("DeactivateHeadbutt", 0.5f);
        }
    }

    public void ActivateHeadbuttCollider()
    {
        if (headbuttColliderObject != null)
        {
            headbuttColliderObject.SetActive(true);
        }
    }


    void PositionHeadbuttCollider()
    {
        if (spriteRenderer.flipX)
        {
            headbuttColliderObject.transform.localPosition = offsetLeft;
        }
        else
        {
            headbuttColliderObject.transform.localPosition = offsetRight;
        }
    }

    void DeactivateHeadbutt()
    {
        if (headbuttColliderObject != null)
        {
            headbuttColliderObject.SetActive(false);
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
