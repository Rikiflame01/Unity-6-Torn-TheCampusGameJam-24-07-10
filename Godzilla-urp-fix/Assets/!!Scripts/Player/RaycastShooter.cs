using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public Transform rayOrigin;
    public float rayDistance = 100f;
    public LineRenderer lineRenderer;

    private Vector3 direction;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        // Initialize direction to the right
        direction = Vector3.right;
    }

    void Update()
    {
        bool directionChanged = false;
        // Update direction based on input
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
            directionChanged = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
            directionChanged = true;
        }

        if (directionChanged || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
        {
            // Only perform raycast and update line renderer when necessary
            if (Input.GetKey(KeyCode.Space))
            {
                ShootRaycast();
            }
            else
            {
                StopLerping();
                lineRenderer.enabled = false;
            }
        }
    }

    void ShootRaycast()
    {
        Ray ray = new Ray(rayOrigin.position, direction);
        RaycastHit hit;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

            LerpColorOnHit lerpScript = hit.transform.GetComponentInParent<LerpColorOnHit>();
            if (lerpScript != null)
            {
                lerpScript.StartLerping();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayDistance);
        }
    }

    void StopLerping()
    {
        LerpColorOnHit[] lerpScripts = FindObjectsOfType<LerpColorOnHit>();
        foreach (LerpColorOnHit lerpScript in lerpScripts)
        {
            lerpScript.StopLerping();
        }
    }
}
