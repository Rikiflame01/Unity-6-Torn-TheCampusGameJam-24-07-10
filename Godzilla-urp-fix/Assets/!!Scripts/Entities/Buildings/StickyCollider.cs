using System;
using System.Collections;
using UnityEngine;

public class StickyCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        StickToObject(collision);
    }

    private void StickToObject(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            transform.SetParent(collision.transform);

            transform.position = collision.contacts[0].point;
        }
        StartCoroutine(DestroyCollider());
    }

    private IEnumerator DestroyCollider()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
