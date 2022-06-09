using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private float _outOfBoundsY = -10;
    
    private List<Vector3> _tiles;
    private bool _teleporting = false;

    private void Start()
    {
        _teleporting = false;
        _tiles = new();

        var floors = FindObjectsOfType<PaintableFloor>();
        floors.ToList().ForEach(f => _tiles.Add(f.transform.position));
    }

    private void Update()
    {
        if(transform.position.y < _outOfBoundsY && !_teleporting)
        {
            _teleporting = true;
            StartCoroutine(StunPlayer(1.5f, GetComponent<PlayerMovement>()));
        }
    }

    public IEnumerator StunPlayer(float stunDuration,
        PlayerMovement player)
    {
        

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _teleporting = false;
        player.BlinkWhileStunned(stunDuration);
        player.Stunned = true;

        StartCoroutine(LerpPlayerToNewPosition(player));
        yield return new WaitForSeconds(stunDuration);
        player.Stunned = false;
    }

    IEnumerator LerpPlayerToNewPosition(PlayerMovement player)
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPosition = GetClosestTile(_tiles, player.transform.position);
        targetPosition.y = 1;
        float elapsedTime = 0;
        float waitTime = .5f;
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        yield return null;
    }

    private Vector3 GetClosestTile(List<Vector3> tiles, Vector3 playerPos)
    {
        Vector3 bestTarget = tiles.First();
        float closestDistance = Mathf.Infinity;

        foreach (Vector3 tile in _tiles)
        {
            float distance = Vector3.Distance(tile, playerPos);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = tile;
            }
        }

        return bestTarget;
    }
}
