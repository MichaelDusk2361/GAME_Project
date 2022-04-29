using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Dictionary<PlayerMovement, Vector3> _fallingPlayers = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerMovement>() is PlayerMovement player)
        {
            if (_fallingPlayers.ContainsKey(player))
            {
                StartCoroutine(StunPlayer(1.5f, player, _fallingPlayers[player]));
            }
            else
            {
                player.Stunned = true;
                _fallingPlayers.Add(player, player.transform.position);
            }
        }
    }

    public IEnumerator StunPlayer(float stunDuration,
        PlayerMovement player, Vector3 entryPosition)
    {
        Vector3 direction = entryPosition - transform.position;

        Vector3 position = transform.position + direction + direction.normalized * 2;
        position.y = 1;
        player.transform.position = position;


        yield return new WaitForSeconds(stunDuration);
        player.Stunned = false;

        _fallingPlayers.Remove(player);
    }
}
