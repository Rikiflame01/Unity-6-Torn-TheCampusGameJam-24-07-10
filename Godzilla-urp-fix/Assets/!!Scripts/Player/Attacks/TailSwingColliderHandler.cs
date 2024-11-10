using UnityEngine;
using UnityEngine.AI;

public class TailSwingColliderHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Damage amount dealt by the tail swing to a tank enemy.")]
    private int tankDamageAmount = 15;

    [SerializeField, Tooltip("Upward force applied to the tank when hit.")]
    private float upwardForce = 1000f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TankEnemy1"))
        {
            IHealth health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(tankDamageAmount);
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
