using UnityEngine;

public class ChargeAttackHandler : MonoBehaviour
{
    public GameObject laserBeamPrefab;
    public Transform firePoint;
    public float chargeCooldown = 5f;
    public float laserForce = 20f;
    public ParticleSystem laserParticles;
    [HideInInspector] public bool isOnCooldown;

    private float cooldownTimer;
    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        cooldownTimer = 0f;
        isOnCooldown = false;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= chargeCooldown)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    public void FireLaser()
    {
        GameObject laserBeam = Instantiate(laserBeamPrefab, firePoint.position, firePoint.rotation);
        ParticleSystem particles = Instantiate(laserParticles, firePoint.position, firePoint.rotation);

        Vector3 direction = playerSpriteRenderer.flipX ? Vector3.left : Vector3.right;
        laserBeam.GetComponent<LaserBeam>().SetDirection(direction, laserForce);

        if (playerSpriteRenderer.flipX)
        {
            particles.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            particles.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        SFXManager.Instance.PlayT_RexAudio("trex");
        SFXManager.Instance.PlayAudioWithVolume("surge", 1.5f);
        particles.Play();

        Destroy(laserBeam, 1.5f);
        Destroy(particles.gameObject, 1.5f);
        isOnCooldown = true;
        EventsManager.Instance.TriggerChargeAttackCooldown();
    }


}
