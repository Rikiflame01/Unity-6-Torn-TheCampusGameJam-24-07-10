using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public Transform targetPoint;      
    public Camera currentCamera;       
    public Camera nextCamera;         
    public float transitionDuration = 2f; 
    public Vector3 finalRotation = new Vector3(20, 0, 0); 

    private bool isTransitioning = false;
    private float transitionTime = 0f;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool hasStartedFinalRotation = false;

    void Start()
    {
        StartCameraTransition();
        if (currentCamera != null) currentCamera.enabled = true;
        if (nextCamera != null) nextCamera.enabled = false;
    }

    public void StartCameraTransition()
    {
        if (!isTransitioning && currentCamera != null && targetPoint != null)
        {
            isTransitioning = true;
            transitionTime = 0f;
            startPosition = currentCamera.transform.position;
            startRotation = currentCamera.transform.rotation;
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            transitionTime += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTime / transitionDuration);

            float easedT = Mathf.Pow(t, 2) * (3f - 2f * t);

            currentCamera.transform.position = Vector3.Lerp(startPosition, targetPoint.position, easedT);

            if (transitionDuration - transitionTime <= 1f && !hasStartedFinalRotation)
            {
                hasStartedFinalRotation = true;
                startRotation = currentCamera.transform.rotation;
            }

            if (hasStartedFinalRotation)
            {
                float rotationT = Mathf.Clamp01((transitionTime - (transitionDuration - 1f)) / 1f);
                currentCamera.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(finalRotation), rotationT);
            }

            if (t >= 1f)
            {
                CompleteTransition();
            }
        }
    }

    void CompleteTransition()
    {
        Debug.Log("Camera transition completed!");

        isTransitioning = false;

        if (currentCamera != null)
        {
            currentCamera.enabled = false;
        }

        if (nextCamera != null)
        {
            nextCamera.enabled = true;
        }
    }
}
