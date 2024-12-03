using UnityEngine;
using UnityEngine.AI;

public class ProjectileTankEnemy : MonoBehaviour
{
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

    private Transform player;
    private float fireCooldown;

    private void Awake()
    {
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
        UpdateClosestPlayer();
        if (player != null)
        {
            MoveTowardsPlayer();
        }

        HandleFiring();
    }

    /// <summary>
    /// Updates the closest player as the target.
    /// </summary>
    private void UpdateClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            player = null;
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject playerObject in players)
        {
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerObject.transform;
            }
        }

        player = closestPlayer;
    }

    /// <summary>
    /// Moves the enemy towards the player, maintaining the specified distance.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
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

    /// <summary>
    /// Handles firing projectiles at intervals.
    /// </summary>
    private void HandleFiring()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f && player != null)
        {
            FireProjectile();
            fireCooldown = fireRate;
        }
    }

    /// <summary>
    /// Fires a projectile from the fire point.
    /// </summary>
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
