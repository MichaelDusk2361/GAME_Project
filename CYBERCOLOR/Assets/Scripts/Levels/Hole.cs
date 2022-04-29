using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private readonly Dictionary<PlayerMovement, Vector3> _fallingPlayers = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerMovement>() is PlayerMovement player)
        {
            if (_fallingPlayers.ContainsKey(player) && player.transform.position.y < -2f)
            {
                StartCoroutine(StunPlayer(1.5f, player, _fallingPlayers[player]));
            }
            else if (!_fallingPlayers.ContainsKey(player))
            {
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
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = position;

        player.Stunned = true;
        yield return new WaitForSeconds(stunDuration);
        player.Stunned = false;

        _fallingPlayers.Remove(player);
    }
}
