using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Singleton { get; private set; }

    [SerializeField] private Material[] _playerMaterials;
    [SerializeField] private Material[] _playerPaintMaterials;
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;

    public List<Vector3> SpawnPoints { get; set; }

    private int _playerIndex;

    void Awake()
    {
        var floorTiles = FindObjectsOfType<PaintableFloor>().ToList();

        SpawnPoints = new List<Vector3>();
        for (int i = 0; i < GetComponent<PlayerInputManager>().maxPlayerCount; i++)
        {
            var tile = floorTiles[Random.Range(0, floorTiles.Count)];
            SpawnPoints.Add(tile.transform.position);
            floorTiles.Remove(tile);
        }

        _playerIndex = 0;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        // Set player properties
        int index = _playerIndex++;
        player.GetComponent<MeshRenderer>().material = _playerMaterials[index];
        player.GetComponent<PlayerPainter>().PaintMaterial = _playerPaintMaterials[index];
        player.gameObject.name = "Player " + _playerIndex.ToString();
        player.transform.position = SpawnPoints[index];
        _cinemachineTargetGroup.AddMember(player.transform, 1, 6.5f);
    }
}
