using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float laserForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 direction, float force)
    {
        moveDirection = direction.normalized;
        laserForce = force;
    }

    void FixedUpdate()
    {
        rb.AddForce(moveDirection * laserForce, ForceMode.Force);
    }
}
