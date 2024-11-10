using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public float homingStrength = 3f;
    public float homingDuration = 2f;
    public LayerMask ignoreLayer;

    private Transform target;
    private float homingTimer = 0f;
    private Quaternion initialRotation;
    private Collider rocketCollider;

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
        homingTimer = 0f;
        target = FindClosestTarget();
        initialRotation = transform.rotation;
        rocketCollider = GetComponent<Collider>();

        if (rocketCollider != null)
        {
            Collider[] colliders = FindObjectsOfType<Collider>();
            foreach (Collider col in colliders)
            {
                if (((1 << col.gameObject.layer) & ignoreLayer) != 0)
                {
                    Physics.IgnoreCollision(rocketCollider, col);
                }
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (homingTimer < homingDuration && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetRotation.eulerAngles.y, initialRotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, homingStrength * Time.deltaTime);
            homingTimer += Time.deltaTime;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, homingStrength * Time.deltaTime);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IHealth>().TakeDamage(20);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.ReturnObject("Rockets", this);
    }

    private Transform FindClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("EnemyTargetPoint");
        return player != null ? player.transform : null;
    }
}
