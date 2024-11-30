using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HeadbuttColliderHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Explosion particle effect prefab.")]
    private GameObject explosionParticlesPrefab;

    [SerializeField, Tooltip("The tag of the objects this collider should ignore.")]
    private string ignoreTag = "Road";

    [SerializeField, Tooltip("Damage amount dealt by the headbutt to a building.")]
    private int buildingDamageAmount = 10;

    [SerializeField, Tooltip("Damage amount dealt by the headbutt to a tank enemy.")]
    private int tankDamageAmount = 15;

    [SerializeField, Tooltip("Damage amount dealt by the headbutt to a helicopter.")]
    private int helicopterDamageAmount = 20;

    [SerializeField, Tooltip("Damage amount dealt by the headbutt to a Jet")]
    private int jetDamageAmount = 25;

    [SerializeField, Tooltip("Additional upward force upon destruction.")]
    private float upwardForce = 500f;

    public float disableDuration = 2.0f;
    public float force = 100f;
    private float lastExplosionTime;
    private float explosionCooldown = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ignoreTag)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }

        IHealth health = collision.gameObject.GetComponent<IHealth>();
        if (health != null)
        {
            switch (collision.gameObject.tag)
            {
                case "Building":
                    ApplyDamageAndHandleDestruction(health, buildingDamageAmount, collision);
                    break;
                case "TankEnemy1":
                    TankDamage(health, tankDamageAmount, collision);
                    break;
                case "HelicopterEnemy1":
                    HelicopterDamager(health, helicopterDamageAmount, collision);
                    break;
                case "JetEnemy1":
                    JetDamager(health, jetDamageAmount, collision);
                    break;
                case "LightHouse":
                    ApplyDamageAndHandleDestruction(health, buildingDamageAmount, collision);
                    break;
                default:
                    break;
            }
        }

        NavMeshAgent navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (navMeshAgent != null && rb != null)
        {
            navMeshAgent.enabled = false;
            ProjectileTankEnemy script = collision.gameObject.GetComponent<ProjectileTankEnemy>();
            if (script != null)
            {
                script.enabled = false;
            }
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            rb.AddForce(Vector3.up * 50);
            rb.AddForce(direction * force, ForceMode.Impulse);

            StartCoroutine(ReenableNavMeshAgent(navMeshAgent, disableDuration));
        }

    }

    private void JetDamager(IHealth health, int jetDamageAmount, Collision collision)
    {
        health.TakeDamage(jetDamageAmount);
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
        foreach (Rigidbody rb in collision.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(Vector3.up * upwardForce);
        }
    }

    private void TankDamage(IHealth health, int damageAmount, Collision collision)
    {
        health.TakeDamage(damageAmount);
    }

    private void HelicopterDamager(IHealth health, int damageAmount, Collision collision)
    {
        health.TakeDamage(damageAmount);

    }

    private IEnumerator ReenableNavMeshAgent(NavMeshAgent navMeshAgent, float delay)
    {
        yield return new WaitForSeconds(delay);
        navMeshAgent.enabled = true;
    }
}
