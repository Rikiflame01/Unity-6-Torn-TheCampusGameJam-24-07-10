using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimationHandler : MonoBehaviour
{
    public Sprite[] sprites;
    public float displayTime = 3f;
    public float fadeDuration = 1f;
    public float animationSpeed = 1f;

    private Image imageComponent;
    private Canvas canvasComponent;
    private Coroutine animationCoroutine;
    private float spriteInterval;

    private void Start()
    {
        imageComponent = GetComponentInChildren<Image>();
        canvasComponent = GetComponent<Canvas>();

        if (imageComponent == null || canvasComponent == null)
        {
            Debug.LogError("Image or Canvas component missing from GameObject.");
            return;
        }

        spriteInterval = (displayTime - 2 * fadeDuration) / sprites.Length / animationSpeed;
        animationCoroutine = StartCoroutine(PlaySpriteAnimation());
    }

    private IEnumerator PlaySpriteAnimation()
    {
        canvasComponent.enabled = true;
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        float elapsedTime = 0f;
        while (elapsedTime < displayTime - fadeDuration)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                imageComponent.sprite = sprites[i];
                yield return new WaitForSeconds(spriteInterval);
                elapsedTime += spriteInterval;
                if (elapsedTime >= displayTime - fadeDuration)
                    break;
            }
        }

        yield return StartCoroutine(Fade(1, 0, fadeDuration));
        canvasComponent.enabled = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color color = imageComponent.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            imageComponent.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        imageComponent.color = color;
    }
}
