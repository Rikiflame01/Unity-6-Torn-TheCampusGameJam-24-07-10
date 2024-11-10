using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtSpriteHandler : MonoBehaviour
{
    public Sprite normalSpriteStage1;
    public Sprite hurtSpriteStage1;
    public Sprite normalSpriteStage2;
    public Sprite hurtSpriteStage2;
    public Sprite normalSpriteStage3;
    public Color flashColor = Color.red;
    public float hurtDuration = 2f;
    public float flashInterval = 0.5f;

    private Image uiImage;
    private Coroutine hurtCoroutine;
    private Coroutine flashCoroutine;
    private int currentHealthStage = 1;
    private Color originalColor;

    private void Start()
    {
        uiImage = GetComponent<Image>();
        originalColor = uiImage.color;
        uiImage.sprite = normalSpriteStage1;
    }

    public void UpdateHealthState(float healthPercentage)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            uiImage.color = originalColor;
        }

        if (healthPercentage < 0.2f)
        {
            currentHealthStage = 3;
            uiImage.sprite = normalSpriteStage3;
            flashCoroutine = StartCoroutine(FlashRed());
        }
        else if (healthPercentage < 0.4f)
        {
            currentHealthStage = 2;
            uiImage.sprite = normalSpriteStage2;
        }
        else
        {
            currentHealthStage = 1;
            uiImage.sprite = normalSpriteStage1;
        }
    }

    public void TriggerHurtSprite()
    {
        if (currentHealthStage == 3)
            return;

        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine = StartCoroutine(ShowHurtSprite());
    }

    private IEnumerator ShowHurtSprite()
    {
        if (currentHealthStage == 1)
        {
            uiImage.sprite = hurtSpriteStage1;
        }
        else if (currentHealthStage == 2)
        {
            uiImage.sprite = hurtSpriteStage2;
        }

        yield return new WaitForSeconds(hurtDuration);

        if (currentHealthStage == 1)
        {
            uiImage.sprite = normalSpriteStage1;
        }
        else if (currentHealthStage == 2)
        {
            uiImage.sprite = normalSpriteStage2;
        }
    }

    private IEnumerator FlashRed()
    {
        while (true)
        {
            uiImage.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            uiImage.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
