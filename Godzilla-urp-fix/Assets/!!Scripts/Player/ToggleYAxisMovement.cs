using UnityEngine;

public class ToggleYAxisMovement : MonoBehaviour
{
    private bool shouldDisableTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                bool isYAxisMovementEnabled = !playerRigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionY);

                if (isYAxisMovementEnabled)
                {
                    playerRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
                }
                else
                {
                    playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                }
            }

            shouldDisableTrigger = true;
        }
    }

    private void Update()
    {
        if (shouldDisableTrigger)
        {
            GetComponent<Collider>().isTrigger = false;
            shouldDisableTrigger = false;
        }
    }
}
