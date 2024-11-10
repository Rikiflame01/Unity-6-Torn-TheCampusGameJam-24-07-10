using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombReceiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JetBomb") == true){
            EventsManager.Instance.OnShakeEvent?.Invoke("large");
            Destroy(other.gameObject);
        }
    }
}
