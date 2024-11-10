using UnityEngine;

public class BannerAirplaneMovement : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    private Vector3 startPosition;
    private bool movingToTarget = true; 
    private Quaternion initialRotation;
    private Quaternion targetRotation; 
    private bool rotating = false; 

    void Start()
    {
        startPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        MoveAirplane();
    }

    void MoveAirplane()
    {
        Vector3 target = movingToTarget ? targetPosition.position : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (!rotating)
            {
                
                rotating = true;
                targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 180f, 0f));
            }
        }


        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                rotating = false;
                movingToTarget = !movingToTarget;
            }
        }
    }
}
