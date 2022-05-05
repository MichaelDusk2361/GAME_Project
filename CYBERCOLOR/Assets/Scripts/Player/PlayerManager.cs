using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Singleton { get; private set; }

    [SerializeField] private Material[] _playerMaterials;
    [SerializeField] private Material[] _playerPaintMaterials;

    public Dictionary<int, Vector3> SpawnPoints { get; set; }

    private int _playerIndex;

    void Awake()
    {
        SpawnPoints = new Dictionary<int, Vector3>();
        _playerIndex = 0;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        // Set player properties
        int index = _playerIndex++;
        player.GetComponent<MeshRenderer>().material = _playerMaterials[index];
        player.GetComponent<PlayerPainter>().PaintMaterial = _playerPaintMaterials[index];

        player.transform.position = SpawnPoints[index];
        Camera.main.GetComponent<CameraControl>().Targets.Add(player.transform);
    }
}
