using UnityEngine;

public class ChildObjectController : MonoBehaviour
{
    public string releaseTag = "Player";

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isReleased = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (!isReleased)
        {
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == releaseTag)
        {
            isReleased = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
