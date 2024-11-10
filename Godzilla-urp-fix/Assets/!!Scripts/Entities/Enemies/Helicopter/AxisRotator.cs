using UnityEngine;

public class AxisRotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 30f;

    void Update()
    {
        RotatePropeller();
    }

    void RotatePropeller()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rotationSpeed = 0;
    }
}
