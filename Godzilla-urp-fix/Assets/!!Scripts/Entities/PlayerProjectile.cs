using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField, Tooltip("The tag of the objects this projectile should ignore.")]
    private string ignoreTag;

    [SerializeField, Tooltip("Damage amount dealt by the projectile to a building.")]
    private int buildingDamageAmount = 10;

    [SerializeField, Tooltip("Damage amount dealt by the projectile to a tank enemy.")]
    private int tankDamageAmount = 15;

    [SerializeField, Tooltip("Damage amount dealt by the projectile to a helicopter.")]
    private int helicopterDamageAmount = 20;

    [SerializeField, Tooltip("Additional upward force upon destruction.")]
    private float upwardForce = 500f;

    [SerializeField, Tooltip("Explosion particle effect prefab.")]
    private GameObject explosionParticlesPrefab;

    public float disableDuration = 2.0f;
    public float force = 100f;
    private float explosionCooldown = 5f;
    private float lastExplosionTime;

    private void OnTriggerEnter(Collider other)
    {
        NavMeshAgent navMeshAgent = other.GetComponent<NavMeshAgent>();
        Rigidbody rb = other.GetComponent<Rigidbody>();

        string collidedTag = other.tag;

        if (collidedTag == ignoreTag)
        {
            return;
        }

        IHealth health = other.GetComponent<IHealth>();
        if (health != null)
        {
            switch (collidedTag)
            {
                case "Building":
                    ApplyDamageAndHandleDestruction(health, buildingDamageAmount, other);
                    EnableAndUnparentFirePrefabs(other.gameObject);
                    break;
                case "TankEnemy1":
                    TankDamage(health, tankDamageAmount, other);
                    break;
                case "Helicopter":
                    HelicopterDamage(health, helicopterDamageAmount, other);
                    break;
                case "LightHouse":
                    ApplyDamageAndHandleDestruction(health, buildingDamageAmount, other);
                    break;
                case "TokyoTower":
                    ApplyDamageAndHandleDestruction(health, buildingDamageAmount, other);
                    break;
                default:
                    Debug.Log("Unhandled tag: " + collidedTag);
                    break;
            }
        }

        if (navMeshAgent != null && rb != null)
        {
            navMeshAgent.enabled = false;
            ProjectileTankEnemy script = other.GetComponent<ProjectileTankEnemy>();
            if (script != null)
            {
                script.enabled = false;
            }

            Vector3 direction = (other.transform.position - transform.position).normalized;
            rb.AddForce(Vector3.up * 50);
            rb.AddForce(direction * force, ForceMode.Impulse);

            StartCoroutine(ReenableNavMeshAgent(navMeshAgent, disableDuration));
        }

        StartCoroutine(DestroyProjectile());
    }

    private void ApplyDamageAndHandleDestruction(IHealth health, int damageAmount, Collider collider)
    {
        health.TakeDamage(damageAmount);

        AddComponentsToChildren addComponents = collider.gameObject.GetComponent<AddComponentsToChildren>();
        if (addComponents != null)
        {
            if (health.GetCurrentHealth() <= 5 && health.GetCurrentHealth() > 3)
            {
                EventsManager.Instance.TriggerShakeEvent("small");
                SFXManager.Instance.PlayAudioWithVolume("thud", 1);
                Debug.Log("Applying Stage 1 components");
                addComponents.ApplyStage1();
                ApplyUpwardForce(collider);
            }
            else if (health.GetCurrentHealth() <= 3 && health.GetCurrentHealth() > 0)
            {
                EventsManager.Instance.TriggerShakeEvent("medium");
                SFXManager.Instance.PlayAudioWithVolume("thud", 2.5f);
                Debug.Log("Applying Stage 2 components");
                addComponents.ApplyStage2();
                ApplyUpwardForce(collider);
            }
            else if (health.GetCurrentHealth() <= 0)
            {
                EventsManager.Instance.TriggerShakeEvent("large");
                SFXManager.Instance.PlayAudioWithVolume("thud", 3.5f);
                addComponents.ApplyStage1();
                ApplyUpwardForce(collider);
                addComponents.ApplyStage2();
                ApplyUpwardForce(collider);
                addComponents.ApplyStage3();
                ApplyUpwardForce(collider);

                Collider[] colliders = collider.gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider childCollider in colliders)
                {
                    childCollider.enabled = false;
                }

                Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }

                collider.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

                if (Time.time >= lastExplosionTime + explosionCooldown)
                {
                    GameObject explosionParticles = Instantiate(explosionParticlesPrefab, collider.transform.position, Quaternion.identity);
                    lastExplosionTime = Time.time;

                    foreach (var ps in explosionParticles.GetComponentsInChildren<ParticleSystem>())
                    {
                        ps.Play();
                    }

                    Destroy(explosionParticles, 3f);
                }

                Collider objectCollider = collider.gameObject.GetComponent<Collider>();
                if (objectCollider != null)
                {
                    Destroy(objectCollider);
                }
                else
                {
                    Debug.Log("No collider found on: " + collider.gameObject.name);
                }
            }
        }
        else
        {
            Debug.LogWarning("AddComponentsToChildren component not found on the object.");
        }
    }
    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void ApplyUpwardForce(Collider collision)
    {
        foreach (Rigidbody rb in collision.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(Vector3.up * upwardForce);
        }
    }

    private void HelicopterDamage(IHealth health, int damageAmount, Collider collider)
    {
        health.TakeDamage(damageAmount);
    }

    private void TankDamage(IHealth health, int damageAmount, Collider collider)
    {
        health.TakeDamage(damageAmount);
    }

    private IEnumerator ReenableNavMeshAgent(NavMeshAgent navMeshAgent, float delay)
    {
        yield return new WaitForSeconds(delay);
        navMeshAgent.enabled = true;
    }

    private void EnableAndUnparentFirePrefabs(GameObject targetObject)
    {
        var fireObjects = targetObject.GetComponentsInChildren<Transform>(true);
        foreach (var fireObject in fireObjects)
        {
            if (fireObject.CompareTag("Fire"))
            {
                fireObject.gameObject.SetActive(true);
                fireObject.SetParent(null);
                FireManager.Instance.AddFirePrefab(fireObject.gameObject);
            }
        }
    }
}
