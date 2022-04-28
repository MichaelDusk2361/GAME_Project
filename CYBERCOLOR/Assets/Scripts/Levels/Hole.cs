using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() is PlayerMovement player)
        {
            StartCoroutine(player.OnStun());
        }
    }
}
