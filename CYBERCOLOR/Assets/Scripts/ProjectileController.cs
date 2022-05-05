using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileState { Charging, Released }

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 14;
    [SerializeField]
    private float _colorRadius = 4;
    public ProjectileState state = ProjectileState.Charging;
    // Start is called before the first frame update
    public PlayerPainter Player { get; set; }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ProjectileState.Charging:
                break;
            case ProjectileState.Released:
                transform.Translate(_projectileSpeed * Time.deltaTime * Vector3.forward);
                break;
            default:
                break;
        }
    }

    public void Init(PlayerPainter player)
    {
        transform.localPosition += Vector3.forward * 1.5f;
        Player = player;
    }
    IEnumerator Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public void Release()
    {
        state = ProjectileState.Released;
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this || other.gameObject == Player.gameObject || state == ProjectileState.Charging)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _colorRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<PaintableFloor>() is PaintableFloor paintableFloor)
            {
                paintableFloor.PaintFloor(Player);
            }
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _colorRadius);
    }
}
