using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWithTags : MonoBehaviour
{
    public List<string> tagsToIgnore = new List<string>();

    private void Start()
    {
        Collider thisCollider = GetComponent<Collider>();
        if (thisCollider == null)
        {
            Debug.LogError("No Collider component found on this GameObject.");
            return;
        }

        foreach (string tag in tagsToIgnore)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                Collider otherCollider = obj.GetComponent<Collider>();
                if (otherCollider != null)
                {
                    Physics.IgnoreCollision(thisCollider, otherCollider);
                }
            }
        }
    }
}
