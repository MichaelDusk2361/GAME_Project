using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Singleton { get; private set; }
    
    public Dictionary<PlayerPainter, int> PlayerScores = new();
    public int TotalFields;

    private void Awake()
    {
        Singleton = this;
    }

    public void AddScore(PlayerPainter player, int score)
    {
        if (!PlayerScores.ContainsKey(player))
        {
            PlayerScores.Add(player, 0);
        }
        PlayerScores[player] += score;
    }

    public void RemoveScore(PlayerPainter player, int score)
    {
        PlayerScores[player] -= score;
    }
}
