using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float hoverHeight = 10f;
    public float hoverForce = 9.81f;
    public float hoverDamping = 2f;
    private Transform player;
    public float minDistanceToPlayerX = 15f;
    public float minDistanceToPlayerZ = 15f;
    public float recoveryHeight = 5f;
    public float recoveryTime = 2f;
    private bool isRecovering = false;
    private Quaternion originalRotation;

    private Rigidbody rb;
    private Collider[] childColliders;

    [Header("Collision Ignoring")]
    public LayerMask ignoreCollisionLayers;

    public bool isMovementDisabled = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        childColliders = GetComponentsInChildren<Collider>();
        originalRotation = transform.rotation;

        IgnoreCollisionLayers();
    }

    void FixedUpdate()
    {
        if (!isRecovering && !isMovementDisabled)
        {
            Hover();
            MoveTowardsPlayer();
            LookAtPlayer();
        }
    }

    void Hover()
    {
        float heightDifference = hoverHeight - transform.position.y;
        float hoverForceAdjusted = hoverForce * heightDifference * hoverDamping;
        rb.AddForce(Vector3.up * hoverForceAdjusted, ForceMode.Acceleration);
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
            float distanceToPlayerZ = Mathf.Abs(transform.position.z - player.position.z);

            Vector3 moveDirection = Vector3.zero;

            if (distanceToPlayerX > minDistanceToPlayerX)
            {
                float moveDirectionX = Mathf.Sign(player.position.x - transform.position.x);
                moveDirection.x = moveDirectionX;
            }

            if (distanceToPlayerZ > minDistanceToPlayerZ)
            {
                float moveDirectionZ = Mathf.Sign(player.position.z - transform.position.z);
                moveDirection.z = moveDirectionZ;
            }

            if (moveDirection != Vector3.zero)
            {
                Vector3 move = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(transform.position + move);
            }
        }
    }

    void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * moveSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isRecovering && !isMovementDisabled)
        {
            // Debug.Log("Helicopter collided with " + collision.gameObject.name);
            StartCoroutine(RecoveryRoutine());
        }
    }

    private System.Collections.IEnumerator RecoveryRoutine()
    {
        if (isMovementDisabled == true)
        {
            yield return null;
        }
        isRecovering = true;
        SetCollidersEnabled(false);

        transform.rotation = originalRotation;

        Vector3 recoveryPosition = transform.position + Vector3.up * recoveryHeight;
        float elapsedTime = 0f;

        while (elapsedTime < recoveryTime)
        {
            transform.position = Vector3.Lerp(transform.position, recoveryPosition, elapsedTime / recoveryTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetCollidersEnabled(true);
        isRecovering = false;
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (var collider in childColliders)
        {
            collider.enabled = enabled;
        }
    }

    private void IgnoreCollisionLayers()
    {
        int helicopterLayer = gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            if ((ignoreCollisionLayers.value & (1 << i)) != 0)
            {
                Physics.IgnoreLayerCollision(helicopterLayer, i, true);
            }
        }
    }

    public void DisableMovementForSeconds(float seconds)
    {
        StartCoroutine(DisableMovementRoutine(seconds));
    }

    private System.Collections.IEnumerator DisableMovementRoutine(float seconds)
    {
        isMovementDisabled = true;
        rb.useGravity = true;
        yield return new WaitForSeconds(seconds);
        isMovementDisabled = false;
        rb.useGravity = false;
    }
}
