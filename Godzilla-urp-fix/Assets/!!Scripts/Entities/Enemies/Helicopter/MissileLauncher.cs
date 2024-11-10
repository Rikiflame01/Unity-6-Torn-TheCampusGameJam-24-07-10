using System.Collections;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform[] firePoints;
    public int burstSize = 3;
    public float burstInterval = 0.5f;
    public float cooldownTime = 15f;
    public int maxAmmoSets = 4;
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

        if (!PoolManager.Instance.HasPool("Missiles"))
        {
            PoolManager.Instance.CreatePool("Missiles", missilePrefab.GetComponent<Missile>(), 50, transform);
        }
        StartCoroutine(AutoFire());
    }

    private IEnumerator AutoFire()
    {
        while (remainingAmmoSets > 0)
        {
            if (!isCooldown)
            {
                StartCoroutine(FireBurst());
                remainingAmmoSets--;
            }
            yield return null;
        }
    }

    private IEnumerator FireBurst()
    {
        isCooldown = true;
        for (int i = 0; i < burstSize; i++)
        {
            FireMissile();
            yield return new WaitForSeconds(burstInterval);
        }
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private void FireMissile()
    {
        Missile missile = PoolManager.Instance.GetObject<Missile>("Missiles");
        missile.transform.position = firePoints[currentFirePointIndex].position;
        missile.transform.rotation = firePoints[currentFirePointIndex].rotation;
        missile.transform.SetParent(null);
        missile.gameObject.SetActive(true);

        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        if (missileRb != null)
        {
            missileRb.AddForce(firePoints[currentFirePointIndex].forward * launchForce, ForceMode.Impulse);
        }

        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    public void ReturnMissile(Missile missile)
    {
        PoolManager.Instance.ReturnObject("Missiles", missile);
    }
}
