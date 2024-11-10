using UnityEngine;

public class LerpColorOnHit : MonoBehaviour
{
    public float lerpSpeed = 2f;
    private Renderer[] childRenderers;
    private Color[] originalColors;
    private bool isLerping;
    private float lerpTime;

    void Start()
    {
        // Get all child renderers
        childRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[childRenderers.Length];
        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalColors[i] = childRenderers[i].material.color;
        }
        enabled = false; // Disable the component by default
    }

    void Update()
    {
        if (isLerping)
        {
            lerpTime += Time.deltaTime * lerpSpeed;
            Color lerpedColor = Color.Lerp(originalColors[0], Color.white, Mathf.PingPong(lerpTime, 1));
            for (int i = 0; i < childRenderers.Length; i++)
            {
                childRenderers[i].material.color = lerpedColor;
            }
        }
    }

    public void StartLerping()
    {
        isLerping = true;
        lerpTime = 0;
        enabled = true; // Enable the component to start calling Update
    }

    public void StopLerping()
    {
        isLerping = false;
        ResetColors();
        enabled = false; // Disable the component to stop calling Update
    }

    private void ResetColors()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.color = originalColors[i];
        }
    }
}
