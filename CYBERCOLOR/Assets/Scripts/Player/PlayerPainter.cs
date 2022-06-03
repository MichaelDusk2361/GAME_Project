using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPainter : MonoBehaviour
{
    public Material PaintMaterial { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PaintableFloor>() is PaintableFloor floor)
        {
            floor.PaintFloor(this);
        }
    }
}
