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
            collision.gameObject.GetComponent<IHealth>().TakeDamage(1);
        }

        ReturnToPool();
    }
}
