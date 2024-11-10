using UnityEngine;

public class DashAttackHandler : MonoBehaviour
{
    public Vector3 offsetRight;
    public Vector3 offsetLeft;

    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 15f;

    public GameObject afterImagePrefab;
    public float afterImageSpawnInterval = 0.05f;
    public float minAfterImageAlpha = 0.2f;
    public float maxAfterImageAlpha = 1f;

    [HideInInspector]
    public bool isDashing = false;

    private Rigidbody rb;
    private float dashTimer = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;
    private float afterImageSpawnTimer = 0f;

    private GameObject dashColliderObject;
    private Collider dashCollider;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform dashColliderTransform = transform.Find("DashCollider");
        if (dashColliderTransform != null)
        {
            dashColliderObject = dashColliderTransform.gameObject;
            dashCollider = dashColliderObject.GetComponent<Collider>();
            if (dashCollider != null)
            {
                dashCollider.enabled = false;
            }
        }
        else
        {
            Debug.LogError("DashCollider child object not found.");
        }
    }


    public void StartDash(Vector3 direction)
    {
        if (Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            if (Random.Range(1, 6) == 1)
            {
                SFXManager.Instance.PlayAudioWithVolume("rare-dash", 6);
            }
            else
            {
                SFXManager.Instance.PlayAudioWithVolume("Dash", 6f);
            }

            isDashing = true;
            dashTimer = 0f;
            lastDashTime = Time.time;
            dashDirection = direction.normalized;

            rb.linearVelocity = dashDirection * dashForce;

            if (dashCollider != null)
            {
                PositionDashCollider();
                dashCollider.enabled = true;
            }

            afterImageSpawnTimer = 0f;

            EventsManager.Instance.TriggerDashAttackCooldown();
        }
    }



    void Update()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            afterImageSpawnTimer += Time.deltaTime;
            if (afterImageSpawnTimer >= afterImageSpawnInterval)
            {
                SpawnAfterImage();
                afterImageSpawnTimer = 0f;
            }

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                rb.linearVelocity = Vector3.zero;

                if (dashCollider != null)
                {
                    dashCollider.enabled = false;
                }
            }
        }
    }

    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        SpriteRenderer afterImageSpriteRenderer = afterImage.GetComponent<SpriteRenderer>();

        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
        afterImageSpriteRenderer.sprite = playerSpriteRenderer.sprite;
        afterImageSpriteRenderer.flipX = playerSpriteRenderer.flipX;

        float progress = dashTimer / dashDuration;
        float alpha = Mathf.Lerp(maxAfterImageAlpha, minAfterImageAlpha, progress);
        Color color = afterImageSpriteRenderer.color;
        color.a = alpha;
        afterImageSpriteRenderer.color = color;
    }

    private void PositionDashCollider()
    {
        if (spriteRenderer.flipX)
        {
            dashColliderObject.transform.localPosition = offsetLeft;
        }
        else
        {
            dashColliderObject.transform.localPosition = offsetRight;
        }
    }

}
