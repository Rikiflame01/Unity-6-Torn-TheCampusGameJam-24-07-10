using UnityEngine;

public class PlayAudioRnd : MonoBehaviour
{
    public AudioClip audioClip; // Assign your audio clip in the Inspector
    public float minInterval = 5f; // Minimum time between audio plays
    public float maxInterval = 15f; // Maximum time between audio plays

    private AudioSource audioSource;
    private float nextPlayTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found! Attach an AudioSource to this GameObject.");
            return;
        }

        // Set the initial play time
        nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlayRandomAudio();
            // Calculate the next play time
            nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    private void PlayRandomAudio()
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned! Please assign an audio clip in the Inspector.");
        }
    }

}
