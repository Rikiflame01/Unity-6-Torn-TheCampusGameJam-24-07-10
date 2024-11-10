using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    private Collider[] colliders;
    private Rigidbody[] rigidbodies;

    void Start()
    {
        // Get all colliders and rigidbodies in the object and its children
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        // Disable all colliders and rigidbodies at the start
        SetComponentsActive(false);
    }

    void OnBecameVisible()
    {
        // Enable colliders and rigidbodies when the object becomes visible
        SetComponentsActive(true);
    }

    void OnBecameInvisible()
    {
        // Disable colliders and rigidbodies when the object becomes invisible
        SetComponentsActive(false);
    }

    void SetComponentsActive(bool isActive)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = isActive;
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = isActive;
            rigidbody.isKinematic = !isActive; // Set to kinematic when not active to avoid physics calculations
        }
    }
}
