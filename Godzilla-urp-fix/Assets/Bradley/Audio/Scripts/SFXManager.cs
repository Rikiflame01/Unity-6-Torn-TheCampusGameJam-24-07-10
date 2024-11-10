using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource aSource;
    private AudioSource tRexSource;
    [SerializeField] private List<AudioClip> clips;
    private Dictionary<string, AudioClip> audioMap;
    [SerializeField] private float tRexPitch = 3.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        aSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        tRexSource = gameObject.AddComponent<AudioSource>();
        tRexSource.outputAudioMixerGroup = aSource.outputAudioMixerGroup;
        tRexSource.pitch = tRexPitch;

        var reverbFilter = tRexSource.gameObject.AddComponent<AudioReverbFilter>();
        reverbFilter.reverbPreset = AudioReverbPreset.Cave;

        audioMap = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in clips)
        {
            if (!audioMap.ContainsKey(clip.name))
            {
                audioMap.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate clip name {clip.name} found. Skipping.");
            }
        }
    }

    public void StopAudio()
    {
        aSource.Stop();
    }

    public void PlayAudio(string name)
    {
        if (audioMap.TryGetValue(name, out var clip))
        {
            aSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Sound " + name + " not found");
        }
    }

    public void PlayAudioWithVolume(string name, float volume)
    {
        if (audioMap.TryGetValue(name, out var clip))
        {
            aSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.Log("Sound " + name + " not found");
        }
    }

    public void PlayT_RexAudio(string name)
    {
        if (audioMap.TryGetValue(name, out var clip))
        {
            tRexSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("T-Rex sound " + name + " not found");
        }
    }
}
