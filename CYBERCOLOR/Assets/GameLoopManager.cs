using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    // In seconds
    public int RoundTime = 90;

    public int CurRoundTime = 90;
    public bool GameStarted = false;
    public bool GameFinished = false;

    public TMPro.TextMeshProUGUI TimerText;

    public GameObject ScoreboardPanel;
    public TMPro.TextMeshProUGUI ScoreboardText;

    private void Awake()
    {
        Time.timeScale = 1;
        GameStarted = false;
        GameFinished = false;
        TimerText.text = RoundTime.ToString();
    }

    private void Update()
    {
        if (GameFinished && Input.anyKeyDown)
        {
            LoadNextRandomLevel();
        }
    }

    public void OnGameStart(PlayerInput obj)
    {
        if (!GameStarted)
        {
            GameStarted = true;

            CurRoundTime = RoundTime;

            InvokeRepeating(nameof(DiminishTime), 1, 1);
        }
    }

    private void DiminishTime()
    {
        CurRoundTime--;
        TimerText.text = CurRoundTime.ToString();

        if (CurRoundTime == 0)
        {
            // Game ends
            var scores = ScoreManager.Singleton.PlayerScores.OrderBy(key => key.Value).Reverse();
            string scoreboardText = "";

            int place = 1;
            foreach (var item in scores)
            {
                var color = item.Key.PaintMaterial.color;
                string colorText = $"#{ColorUtility.ToHtmlStringRGB(color)}";
                scoreboardText += $"<color={colorText}>{place}. {item.Key.name} - {item.Value} Fields</color>\n";
                place++;
            }
            ScoreboardText.text = scoreboardText;
            ScoreboardPanel.SetActive(true);

            GameFinished = true;
            Time.timeScale = 0;
        }
    }

    public void LoadNextRandomLevel()
    {
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}
