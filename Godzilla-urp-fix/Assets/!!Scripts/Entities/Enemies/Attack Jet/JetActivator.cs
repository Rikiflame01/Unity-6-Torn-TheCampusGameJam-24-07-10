using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsManager.Instance.OnPlayerTriggerJetWarning?.Invoke("JetWarning");
            StartCoroutine(PlayJetAudio());
        }
    }

    private IEnumerator PlayJetAudio()
    {
        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayAudioWithVolume("jet", 20);
        this.gameObject.SetActive(false);
    }
}
