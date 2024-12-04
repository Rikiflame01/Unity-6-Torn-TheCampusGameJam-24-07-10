using UnityEngine;
using UnityEngine.AI;

public class TailSwingColliderHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Damage amount dealt by the tail swing to a tank enemy.")]
    private int tankDamageAmount = 15;

    [SerializeField, Tooltip("Upward force applied to the tank when hit.")]
    private float upwardForce = 1000f;

    private float lastExplosionTime = 0f;
    [SerializeField, Tooltip("Cooldown time between explosions.")]
    private float explosionCooldown = 2f;

    [SerializeField, Tooltip("Prefab for the explosion particles.")]
    private GameObject explosionParticlesPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TankEnemy1"))
        {
            IHealth health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(tankDamageAmount);
                AddComponentsToChildren addComponents = collision.gameObject.GetComponent<AddComponentsToChildren>();
                addComponents.ApplyStage1();
                addComponents.ApplyStage2();
                addComponents.ApplyStage3();
                ApplyUpwardForce(collision);
            }
        }
        if (collision.gameObject.CompareTag("HelicopterEnemy1"))
        {
            IHealth health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
        if (collision.gameObject.CompareTag("Building"))
        {
            IHealth health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                ApplyDamageAndHandleDestruction(health, 5, collision);
            }
        }
    }

    private void ApplyDamageAndHandleDestruction(IHealth health, int damageAmount, Collision collision)
    {
        health.TakeDamage(damageAmount);

        AddComponentsToChildren addComponents = collision.gameObject.GetComponent<AddComponentsToChildren>();
        if (addComponents != null)
        {
            if (health.GetCurrentHealth() <= 5 && health.GetCurrentHealth() > 3)
            {
                EventsManager.Instance.TriggerShakeEvent("small");
                addComponents.ApplyStage1();
                ApplyUpwardForce(collision);
            }
            else if (health.GetCurrentHealth() <= 3 && health.GetCurrentHealth() > 0)
            {
                EventsManager.Instance.TriggerShakeEvent("medium");
                addComponents.ApplyStage2();
                ApplyUpwardForce(collision);
            }
            else if (health.GetCurrentHealth() <= 0)
            {
                SFXManager.Instance.PlayAudioWithVolume("explosion", 3);
                EventsManager.Instance.TriggerShakeEvent("large");
                addComponents.ApplyStage1(); // Apply Stage 1 first
                ApplyUpwardForce(collision);
                addComponents.ApplyStage2(); // Apply Stage 2 next
                ApplyUpwardForce(collision);
                addComponents.ApplyStage3(); // Apply Stage 3 last
                ApplyUpwardForce(collision);

                if (Time.time >= lastExplosionTime + explosionCooldown)
                {
                    GameObject explosionParticles = Instantiate(explosionParticlesPrefab, collision.transform.position, Quaternion.identity);
                    lastExplosionTime = Time.time;

                    foreach (var ps in explosionParticles.GetComponentsInChildren<ParticleSystem>())
                    {
                        ps.Play();
                    }

                    Destroy(explosionParticles, 3f);
                }

                Collider ObjectCollider = collision.gameObject.GetComponent<Collider>();
                if (ObjectCollider != null)
                {
                    Destroy(ObjectCollider);
                }
                else
                {
                    Debug.Log("No collider found on: " + collision.gameObject.name);
                }

                
            }

        }
        else
        {
            Debug.LogWarning("AddComponentsToChildren component not found on the object.");
        }
    }


    private void ApplyUpwardForce(Collision collision)
    {
        NavMeshAgent navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        }
    }
}
