using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public ButtonManager buttonManager;
    public GifPlayer gifPlayer;
    public float fastSpeedDuration = 2f;
    public float fadeDuration = 1f; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayGifFasterAndTransition("GameScene"));
            SFXManager.Instance.PlayT_RexAudio("trex");
            SFXManager.Instance.PlayAudioWithVolume("surge", 1.5f);
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            StartCoroutine(PlayGifFasterAndTransition("GameSceneMultiPlayer"));
            SFXManager.Instance.PlayT_RexAudio("trex");
            SFXManager.Instance.PlayAudioWithVolume("surge", 1.5f);
        }
    }

    private IEnumerator PlayGifFasterAndTransition(string gameScene)
    {
        gifPlayer.SetFrameRate(0.1f);

        yield return new WaitForSeconds(fastSpeedDuration);


        yield return StartCoroutine(FadeOut());

        buttonManager.LoadSceneAsync(gameScene);
    }

    private IEnumerator FadeOut()
    {
        GameObject fadeOverlay = new GameObject("FadeOverlay");
        Canvas canvas = fadeOverlay.AddComponent<Canvas>();
        fadeOverlay.AddComponent<CanvasRenderer>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        UnityEngine.UI.Image image = fadeOverlay.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0, 0, 0, 0);

        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
