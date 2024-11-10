using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Assign a UI Text element to display FPS
    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;
    private float updateRate = 1.0f; // Number of updates per second

    void Start()
    {
        if (fpsText == null)
        {
            Debug.LogError("FPSDisplay: No Text component assigned to fpsText.");
        }
    }

    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;

            // Display the FPS
            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps:F2}";
            }
        }
    }
}
