using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    void Start()
    {
        var playerManager = FindObjectOfType<PlayerManager>();

        if(playerManager == null)
        {
            Debug.LogError("No player manager in scene!");
            return;
        }

        // Set spawnpoint
        Vector3 position = transform.position;
        position.y = 1;
        playerManager.SpawnPoints.Add(playerManager.SpawnPoints.Count, position);
    }
}
