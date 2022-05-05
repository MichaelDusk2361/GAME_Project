using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsReleased { get; set; } = false;
    public PlayerPainter Player { get; set; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReleased)
        {
            transform.position += transform.forward * 14 * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (IsReleased)
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position - new Vector3(0, 1.5f, 0), gameObject.GetComponent<SphereCollider>().bounds.size.y);
            
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<PaintableFloor>() is PaintableFloor paintableFloor)
                {
                    paintableFloor.PaintFloor(Player);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         Destroy (gameObject);
    }

}
