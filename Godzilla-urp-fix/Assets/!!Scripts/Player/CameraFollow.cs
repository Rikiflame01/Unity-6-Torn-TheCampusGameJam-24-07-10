using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float xOffset = 5f;

    public float smoothSpeed = 0.125f;

    public float fixedYPosition = 5f;
    public float fixedZPosition = -10f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = new Vector3(target.position.x + xOffset, fixedYPosition, fixedZPosition);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
