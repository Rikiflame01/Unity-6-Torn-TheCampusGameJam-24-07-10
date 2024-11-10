using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingScreen;
    public Slider progressBar;

    public void EnableObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameObject reference is null.");
        }
    }
    public void DisableObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameObject reference is null.");
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LoadSceneAsync(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            yield return null;
        }
    }

    public void close()
    {
        Application.Quit();
    }
}
