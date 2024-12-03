using UnityEngine;

public class HelicopterMachineGunBullet : MonoBehaviour
{
    public float force = 500f;
    public float lifetime = 2f;
    public LayerMask ignoreLayer;

    public GameObject vfxPrefab;
    public Vector3 vfxScale = Vector3.one;

    private Rigidbody rb;
    private Collider bulletCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
    }

    public void Initialize(Vector3 direction)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifetime);

        IgnoreSpecificCollisions();

        Transform nearestPlayer = FindNearestPlayer();
        if (nearestPlayer != null)
        {
            CalculateNearestPlayerImpact(nearestPlayer);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.ReturnObject("Bullets", this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SFXManager.Instance.PlayAudio("Hitmark");

            if (vfxPrefab != null)
            {
                GameObject vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
                vfx.transform.localScale = vfxScale;
                Destroy(vfx, 1f);
            }

            var health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
        }

        ReturnToPool();
    }

    /// <summary>
    /// Ignores collisions with specified layers.
    /// </summary>
    private void IgnoreSpecificCollisions()
    {
        if (bulletCollider != null)
        {
            Collider[] colliders = FindObjectsOfType<Collider>();
            foreach (Collider col in colliders)
            {
                if (((1 << col.gameObject.layer) & ignoreLayer) != 0)
                {
                    Physics.IgnoreCollision(bulletCollider, col);
                }
            }
        }
    }

    /// <summary>
    /// Finds the nearest player in the scene.
    /// </summary>
    /// <returns>The Transform of the nearest player or null if none found.</returns>
    private Transform FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            return null;
        }

        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        return closestPlayer;
    }

    /// <summary>
    /// Calculates the impact or trajectory adjustment for the nearest player.
    /// </summary>
    /// <param name="nearestPlayer">The Transform of the nearest player.</param>
    private void CalculateNearestPlayerImpact(Transform nearestPlayer)
    {
        // Example calculation: Adjust trajectory toward the nearest player
        Vector3 directionToPlayer = (nearestPlayer.position - transform.position).normalized;

        rb.linearVelocity = Vector3.zero; // Reset current velocity
        rb.AddForce(directionToPlayer * force, ForceMode.Impulse);

        // Optional: Add debugging information
        Debug.DrawLine(transform.position, nearestPlayer.position, Color.red, lifetime);
    }
}
