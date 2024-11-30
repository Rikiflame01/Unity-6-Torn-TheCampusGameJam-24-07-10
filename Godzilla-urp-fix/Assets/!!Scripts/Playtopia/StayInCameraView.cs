using UnityEngine;

public class KeepInCameraView : MonoBehaviour
{
    public Camera targetCamera;
    public float edgePadding = 0.1f;
    public float correctionSpeed = 5f;

    void Update()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No camera assigned to KeepInCameraView script.");
            return;
        }

        // Get the object's position in viewport space
        Vector3 viewportPosition = targetCamera.WorldToViewportPoint(transform.position);

        // Check if the player is outside the camera's view
        if (viewportPosition.x < 0f + edgePadding || viewportPosition.x > 1f - edgePadding ||
            viewportPosition.y < 0f + edgePadding || viewportPosition.y > 1f - edgePadding)
        {
            // Clamp the position to stay within bounds
            viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0f + edgePadding, 1f - edgePadding);
            viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0f + edgePadding, 1f - edgePadding);

            // Convert the clamped position back to world space
            Vector3 targetWorldPosition = targetCamera.ViewportToWorldPoint(viewportPosition);

            // Smoothly move the player back into bounds
            transform.position = Vector3.Lerp(transform.position, targetWorldPosition, correctionSpeed * Time.deltaTime);
        }
    }
}
