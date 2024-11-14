using UnityEngine;

public class HeadbuttAttackHandler : MonoBehaviour
{
    public GameObject playerObject;
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

    void ActivateHeadbutt(GameObject gameObject)
    {
        if (gameObject != playerObject)
        {
            return;
        }
        if (!isOnCooldown && headbuttColliderObject != null)
        {
            isOnCooldown = true;
            EventsManager.Instance.TriggerHeadbuttCooldown();

            PositionHeadbuttCollider();
            Invoke("ActivateHeadbuttCollider", 0.1f);
        }
    }

    public void ActivateHeadbuttCollider()
    {
        if (headbuttColliderObject != null)
        {
            headbuttColliderObject.SetActive(true);
            Invoke("DeactivateHeadbutt", 0.5f);
        }
    }

    public void PositionHeadbuttCollider()
    {
        Debug.Log("Positioning Headbutt Collider");
        if (spriteRenderer.flipX)
        {
            headbuttColliderObject.transform.localPosition = offsetLeft;
            Debug.Log("Headbutt Attack Left");
        }
        else
        {
            headbuttColliderObject.transform.localPosition = offsetRight;
            Debug.Log("Headbutt Attack Right");
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