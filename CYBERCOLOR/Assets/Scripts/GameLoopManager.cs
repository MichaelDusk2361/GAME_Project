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
    bool _gameCanStart = false;

    public TMPro.TextMeshProUGUI TimerText;
    public TMPro.TextMeshProUGUI StartTimerText;
    public GameObject Timer;

    public GameObject ScoreboardPanel;
    public GameObject IngameUI;
    [SerializeField] AudioClip _gameMusic;
    AudioSource _audioSource;
    public TMPro.TextMeshProUGUI ScoreboardText;

    private void Awake()
    {
        Time.timeScale = 0;
        GameStarted = false;
        GameFinished = false;
        IngameUI.SetActive(false);
        TimerText.text = RoundTime.ToString();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _gameMusic;
    }
    bool _inputBlocked = false;
    private void Update()
    {
        if (GameFinished && !_inputBlocked && Input.anyKeyDown)
        {
            LoadNextRandomLevel();
        }
    }

    public void OnGameStart(PlayerInput obj)
    {
        if (!_gameCanStart)
        {
            _gameCanStart = true;
            StartCoroutine(StartTimer());
           
        }
    }
    IEnumerator StartTimer()
    {
        StartTimerText.text = "10";
        for (int i = 9; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);
            StartTimerText.text = i.ToString();
        }
        Timer.SetActive(false);
        IngameUI.SetActive(true);
        _audioSource.Play();
        CurRoundTime = RoundTime;
        Time.timeScale = 1;
        InvokeRepeating(nameof(DiminishTime), 1, 1);
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
            StartCoroutine(BlockInput());
            GameFinished = true;
            Time.timeScale = 0;
        }
    }

    IEnumerator BlockInput()
    {
        _inputBlocked = true;
        yield return new WaitForSecondsRealtime(1);
        _inputBlocked = false;
    }

    public void LoadNextRandomLevel()
    {
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}
