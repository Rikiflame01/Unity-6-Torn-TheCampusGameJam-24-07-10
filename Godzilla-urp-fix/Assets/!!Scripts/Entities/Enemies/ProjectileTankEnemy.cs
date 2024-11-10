using UnityEngine;
using UnityEngine.AI;

public class ProjectileTankEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("The player target.")]
    private Transform player;

    [SerializeField, Tooltip("The navmesh agent for movement.")]
    private NavMeshAgent agent;

    [SerializeField, Tooltip("The point from which projectiles will be fired.")]
    private Transform firePoint;

    [SerializeField, Tooltip("The projectile prefab.")]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("The adjustable distance to keep from the player.")]
    private float keepDistance = 5f;

    [SerializeField, Tooltip("The fire rate in seconds.")]
    private float fireRate = 5f;

    [SerializeField, Tooltip("The force applied to the projectile.")]
    private float projectileForce = 10f;

    private float fireCooldown;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player tag not found. Please ensure a GameObject with the 'Player' tag exists in the scene.");
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        fireCooldown = fireRate;
    }

    private void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
        }

        HandleFiring();
    }

    private void MoveTowardsPlayer()
    {
        if (agent == null)
        {
            return;
        }

        if (!agent.isActiveAndEnabled)
        {
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Destroy(this);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > keepDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    private void HandleFiring()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            FireProjectile();
            fireCooldown = fireRate;
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
            }
        }
    }
}
