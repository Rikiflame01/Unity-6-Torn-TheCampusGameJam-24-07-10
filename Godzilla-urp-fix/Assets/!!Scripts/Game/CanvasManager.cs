using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject winUI;
    public GameObject pauseUI;

    private bool isPaused = false;
    private bool gameEnded = false;

    private void OnEnable()
    {
        EventsManager.Instance.OnPlayerDead.AddListener(OnPlayerDead);
        EventsManager.Instance.OnWinEvent.AddListener(OnPlayerWin);
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnPlayerDead.RemoveListener(OnPlayerDead);
        EventsManager.Instance.OnWinEvent.RemoveListener(OnPlayerWin);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (gameEnded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnPlayerDead(string eventType)
    {
        if (eventType == "PlayerDead")
        {
            gameOverUI.SetActive(true);
            gameEnded = true;
        }
    }

    private void OnPlayerWin(string eventType)
    {
        if (eventType == "PlayerWin")
        {
            winUI.SetActive(true);
            gameEnded = true;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
