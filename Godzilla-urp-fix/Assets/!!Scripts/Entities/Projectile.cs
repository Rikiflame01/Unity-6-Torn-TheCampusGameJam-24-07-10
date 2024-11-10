using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Tooltip("The amount of damage this projectile deals.")]
    private int damageAmount = 1;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SFXManager.Instance.PlayAudioWithVolume("Hitmark", 2.5f);
            IHealth health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
        StartCoroutine(DestroyProjectile());

    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
