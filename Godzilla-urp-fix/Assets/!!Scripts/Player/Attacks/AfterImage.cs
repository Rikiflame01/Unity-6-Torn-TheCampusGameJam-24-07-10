using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(initialColor.a, 0f, timer / fadeDuration);
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
