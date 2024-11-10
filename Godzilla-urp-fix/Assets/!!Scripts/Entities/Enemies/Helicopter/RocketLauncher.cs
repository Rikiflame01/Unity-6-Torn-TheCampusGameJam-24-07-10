using System.Collections;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform[] firePoints;
    public float activationTime = 20f;
    public float cooldownTime = 15f;
    public int maxAmmoSets = 2;
    public float launchForce = 1000f;

    private bool isCooldown = false;
    private int currentFirePointIndex = 0;
    private int remainingAmmoSets;

    private void Start()
    {
        if (firePoints.Length != 2)
        {
            Debug.LogError("Please assign exactly 2 firing points.");
            return;
        }

        remainingAmmoSets = maxAmmoSets;

        if (!PoolManager.Instance.HasPool("Rockets"))
        {
            PoolManager.Instance.CreatePool("Rockets", rocketPrefab.GetComponent<Rocket>(), 20, transform);
        }

        StartCoroutine(AutoFire());
    }

    private IEnumerator AutoFire()
    {
        yield return new WaitForSeconds(activationTime);
        while (remainingAmmoSets > 0)
        {
            if (!isCooldown)
            {
                FireRocket();
                remainingAmmoSets--;
                yield return new WaitForSeconds(cooldownTime);
            }
            yield return null;
        }
    }

    private void FireRocket()
    {
        Rocket rocket = PoolManager.Instance.GetObject<Rocket>("Rockets");
        rocket.transform.position = firePoints[currentFirePointIndex].position;
        rocket.transform.rotation = firePoints[currentFirePointIndex].rotation;
        rocket.transform.SetParent(null);
        rocket.gameObject.SetActive(true);

        Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
        if (rocketRb != null)
        {
            rocketRb.AddForce(firePoints[currentFirePointIndex].forward * launchForce, ForceMode.Impulse);
        }

        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    public void ReturnRocket(Rocket rocket)
    {
        PoolManager.Instance.ReturnObject("Rockets", rocket);
    }
}
