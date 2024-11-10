using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDownwards : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearDamping = 0;
        rb.angularDamping = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        rb.linearVelocity = new Vector3(0, Mathf.Min(rb.linearVelocity.y, 0), 0);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(0, Mathf.Min(rb.linearVelocity.y, -Mathf.Abs(rb.linearVelocity.y)), 0);
    }
}
