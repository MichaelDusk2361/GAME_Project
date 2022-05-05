using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableFloor : MonoBehaviour
{
    public PlayerPainter LastPaintedPlayer { get; set; }
    private MeshRenderer _meshRenderer;

    void Start()
    {
        ScoreManager.Singleton.TotalFields++;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.GetComponent<PlayerPainter>() is PlayerPainter player)
        {
            PaintFloor(player);
        }
    }

    public void PaintFloor(PlayerPainter player)
    {
        if (LastPaintedPlayer != null)
            ScoreManager.Singleton.RemoveScore(LastPaintedPlayer, 1);
        ScoreManager.Singleton.AddScore(player, 1);

        LastPaintedPlayer = player;
        _meshRenderer.material = LastPaintedPlayer.PaintMaterial;
    }
}
