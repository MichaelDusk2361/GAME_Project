using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPainter : MonoBehaviour
{
    public Material PaintMaterial { get; set; }
    [SerializeField] float _paintRadius = 0.4f;
    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.down, _paintRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<PaintableFloor>() is PaintableFloor paintableFloor)
            {
                paintableFloor.PaintFloor(this);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.down, _paintRadius);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponent<PaintableFloor>() is PaintableFloor floor)
    //    {
    //        floor.PaintFloor(this);
    //    }
    //}
}
