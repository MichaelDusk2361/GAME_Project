using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsReleased { get; set; } = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReleased)
            transform.position += transform.forward * 14 * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
         Destroy (gameObject);
    }

}
