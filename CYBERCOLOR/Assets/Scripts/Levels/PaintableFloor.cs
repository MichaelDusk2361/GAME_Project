using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableFloor : MonoBehaviour
{
    public PlayerPainter LastPaintedPlayer;
    private MeshRenderer _mr;

    void Start()
    {
        ScoreManager.Singleton.TotalFields++;
        _mr = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerPainter>() is PlayerPainter player)
        {
            if(LastPaintedPlayer != null)
                ScoreManager.Singleton.RemoveScore(LastPaintedPlayer, 1);
            ScoreManager.Singleton.AddScore(player, 1);

            LastPaintedPlayer = player;
            _mr.material = LastPaintedPlayer.PaintMaterial;
        }
    }
}
