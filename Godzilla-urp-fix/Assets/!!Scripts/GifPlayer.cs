using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GifPlayer : MonoBehaviour
{
    public Image imageComponent;
    public Sprite baseImage;
    public Sprite[] frames;
    public float frameRate = 0.1f;
    private bool isPlaying = false;

    private void Start()
    {
        imageComponent.sprite = baseImage;
        PlayGif();
    }

    public void PlayGif()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayGifCoroutine());
        }
    }

    public void SetFrameRate(float newFrameRate)
    {
        frameRate = newFrameRate;
    }


    private IEnumerator PlayGifCoroutine()
    {
        isPlaying = true;

        while (true)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                imageComponent.sprite = frames[i];
                yield return new WaitForSeconds(frameRate);
            }
        }
    }

    public void StopGif()
    {
        StopCoroutine(PlayGifCoroutine());
        imageComponent.sprite = baseImage;
        isPlaying = false;
    }
}
