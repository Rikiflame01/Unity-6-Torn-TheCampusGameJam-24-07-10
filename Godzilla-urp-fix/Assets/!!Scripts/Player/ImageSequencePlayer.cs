using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageSequencePlayer : MonoBehaviour
{
    [Header("Settings")]

    public CanvasGroup canvasGroup;
    public Image displayImage;          
    public Sprite[] imageArray;         
    public int imagesPerSet = 3;        
    public float imageDisplayTime = 1f; 
    public float pauseDuration = 2f;    

    private int currentIndex = 0;       
    private bool isPlaying = true;

    void Start()
    {

        if (imageArray.Length > 0 && displayImage != null)
        {
            StartCoroutine(PlayImageSequence());
        }
        else
        {
            Debug.LogWarning("Image array or displayImage is not set.");
        }
    }

    private IEnumerator PlayImageSequence()
    {
        yield return new WaitForSeconds(5f);
        canvasGroup.alpha = 1f;
        while (isPlaying)
        {
            for (int i = 0; i < imagesPerSet; i++)
            {
                if (currentIndex >= imageArray.Length)
                {
                    gameObject.SetActive(false);
                }

                displayImage.sprite = imageArray[currentIndex];
                currentIndex++;
                yield return new WaitForSeconds(imageDisplayTime);
            }

            yield return new WaitForSeconds(pauseDuration);
        }
    }

    public void StopPlayback()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    public void ResumePlayback()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlayImageSequence());
        }
    }
}
