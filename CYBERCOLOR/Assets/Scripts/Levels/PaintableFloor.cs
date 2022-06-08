using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableFloor : MonoBehaviour
{
    public PlayerPainter LastPaintedPlayer { get; set; }
    private MeshRenderer _meshRenderer;
    private AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    static int activeSounds = 0;

    void Start()
    {
        ScoreManager.Singleton.TotalFields++;
        _meshRenderer = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PaintFloor(PlayerPainter player)
    {
        if (LastPaintedPlayer != null)
            ScoreManager.Singleton.RemoveScore(LastPaintedPlayer, 1);
        ScoreManager.Singleton.AddScore(player, 1);
        if (LastPaintedPlayer != player)
        {
            if (activeSounds < 3)
            {
                AudioClip selectedClip = _audioClips[Random.Range(0, _audioClips.Length - 1)];
                _audioSource.PlayOneShot(selectedClip);
                activeSounds++;
                StartCoroutine(WaitForClipEnd(selectedClip.length));
            }
        }
        LastPaintedPlayer = player;
        _meshRenderer.material = LastPaintedPlayer.PaintMaterial;
    }

    IEnumerator WaitForClipEnd(float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);
        activeSounds--;
    }
}
