using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component
    private ScoreTextShaker scoreTextShaker;

    private Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();
    private int totalScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        scoreTextShaker = scoreText.GetComponent<ScoreTextShaker>();
    }

    private void OnEnable()
    {
        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.OnDiedEvent += OnObjectDied;
        }
    }

    private void OnDisable()
    {
        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.OnDiedEvent -= OnObjectDied;
        }
    }

    private void OnObjectDied(string objectId, string objectTag)
    {
        if (objectTag == "Building")
        {
            if (scoreDictionary.ContainsKey(objectId))
            {
                scoreDictionary[objectId]++;
            }
            else
            {
                scoreDictionary[objectId] = 1;
            }

            totalScore++;
            UpdateScoreText();

            // Shake the score text
            if (scoreTextShaker != null)
            {
                scoreTextShaker.ShakeText();
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{totalScore}";
    }

    public int GetScore(string objectId)
    {
        if (scoreDictionary.ContainsKey(objectId))
        {
            return scoreDictionary[objectId];
        }

        return 0;
    }
}
