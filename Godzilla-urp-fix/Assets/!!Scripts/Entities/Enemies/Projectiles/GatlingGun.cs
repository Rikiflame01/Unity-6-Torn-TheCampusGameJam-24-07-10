using System.Collections;
using UnityEngine;

public class GatlingGun : MonoBehaviour
{
    private HelicopterController helicopterController;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public int burstSize = 10;
    public float burstInterval = 0.1f;
    public float cooldownTime = 2f;
    public Vector3 rotationOffset;

    private Transform player;
    private bool isCooldown = false;

    private void Start()
    {
        helicopterController = GetComponent<HelicopterController>();
        player = GameObject.FindGameObjectWithTag("EnemyTargetPoint").transform;

        if (firePoint == null)
        {
            Debug.LogError("Please assign a firing point.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Please assign the player's transform.");
            return;
        }

        if (!PoolManager.Instance.HasPool("Bullets"))
        {
            PoolManager.Instance.CreatePool("Bullets", bulletPrefab.GetComponent<HelicopterMachineGunBullet>(), 200, transform);
        }

        StartCoroutine(AutoFire());
    }

    private IEnumerator AutoFire()
    {
        while (!helicopterController.isMovementDisabled)
        {
            if (!isCooldown)
            {
                SFXManager.Instance.PlayAudioWithVolume("Machine gun 8-bit", 1.5f);
                StartCoroutine(FireBurst());
            }
            yield return null;
        }
    }

    private IEnumerator FireBurst()
    {
        isCooldown = true;
        for (int i = 0; i < burstSize; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(burstInterval);
        }
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private void FireBullet()
    {
        HelicopterMachineGunBullet bullet = PoolManager.Instance.GetObject<HelicopterMachineGunBullet>("Bullets");
        bullet.transform.position = firePoint.position;

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        bullet.transform.rotation = targetRotation * Quaternion.Euler(rotationOffset);

        bullet.transform.SetParent(null);
        bullet.gameObject.SetActive(true);

        bullet.Initialize(directionToPlayer);
    }


    public void ReturnBullet(HelicopterMachineGunBullet bullet)
    {
        PoolManager.Instance.ReturnObject("Bullets", bullet);
    }
}
