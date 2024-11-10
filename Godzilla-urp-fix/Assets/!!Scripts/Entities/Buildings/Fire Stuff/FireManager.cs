using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance;
    public List<FirePrefab> firePrefabs = new List<FirePrefab>();
    public List<Sprite> fireAnimationFrames;
    public float frameRate = 0.1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CacheFirePrefabs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CacheFirePrefabs()
    {
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fire");
        foreach (GameObject fireObject in fireObjects)
        {
            FirePrefab firePrefab = new FirePrefab
            {
                GameObject = fireObject,
                SpriteRenderer = fireObject.GetComponent<SpriteRenderer>(),
                SmokeEffects = fireObject.GetComponentsInChildren<ParticleSystem>()
            };
            firePrefabs.Add(firePrefab);
            DisableFirePrefab(firePrefab);
        }
    }

    private void DisableFirePrefab(FirePrefab firePrefab)
    {
        if (firePrefab.SpriteRenderer != null)
        {
            firePrefab.SpriteRenderer.enabled = false;
        }

        foreach (var smokeEffect in firePrefab.SmokeEffects)
        {
            smokeEffect.gameObject.SetActive(false);
        }

        firePrefab.GameObject.SetActive(false);
    }

    public void EnableFirePrefab(FirePrefab firePrefab)
    {

        firePrefab.GameObject.SetActive(true);
        if (firePrefab.SpriteRenderer != null)
        {
            firePrefab.SpriteRenderer.enabled = true;
        }

        foreach (var smokeEffect in firePrefab.SmokeEffects)
        {
            smokeEffect.gameObject.SetActive(true);
            smokeEffect.Play();
        }

        if (firePrefab.SpriteRenderer != null)
        {
            StartCoroutine(PlayFireAnimation(firePrefab));
        }
    }

    public void EnableFirePrefabsAndStartAnimation()
    {
        foreach (var firePrefab in firePrefabs)
        {
            EnableFirePrefab(firePrefab);
        }
    }

    public void AddFirePrefab(GameObject fireObject)
    {
        FirePrefab firePrefab = new FirePrefab
        {
            GameObject = fireObject,
            SpriteRenderer = fireObject.GetComponent<SpriteRenderer>(),
            SmokeEffects = fireObject.GetComponentsInChildren<ParticleSystem>()
        };
        firePrefabs.Add(firePrefab);
        EnableFirePrefab(firePrefab);
    }

    private IEnumerator PlayFireAnimation(FirePrefab firePrefab)
    {
        int frameIndex = 0;
        if (firePrefab.GameObject != null)
        {
            while (firePrefab.GameObject.activeInHierarchy)
            {
                firePrefab.SpriteRenderer.sprite = fireAnimationFrames[frameIndex];
                frameIndex = (frameIndex + 1) % fireAnimationFrames.Count;
                yield return new WaitForSeconds(frameRate);
            }
        }

    }
}

public struct FirePrefab
{
    public GameObject GameObject;
    public SpriteRenderer SpriteRenderer;
    public ParticleSystem[] SmokeEffects;
}
