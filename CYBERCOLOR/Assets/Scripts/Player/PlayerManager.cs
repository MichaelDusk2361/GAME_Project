using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Material[] _playerMaterials;
    public Dictionary<int, Vector3> SpawnPoints;

    private PlayerInputManager _playerInputManager;
    private int _playerIndex;

    void Awake()
    {
        SpawnPoints = new Dictionary<int, Vector3>();
    }

    void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerIndex = 0;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        // Set player properties
        int index = _playerIndex++;
        player.GetComponent<MeshRenderer>().material = _playerMaterials[index];

        player.transform.position = SpawnPoints[index];
    }
}
