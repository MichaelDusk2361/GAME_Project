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
        Vector3 position = GetClosestTile(_tiles, player.transform.position);

        position.y = 1;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = position;
        _teleporting = false;

        player.Stunned = true;
        yield return new WaitForSeconds(stunDuration);
        player.Stunned = false;
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
