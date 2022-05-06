using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileState { Charging, Released }

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 14;
    [SerializeField] private float _colorRadius = 4;
    private float _sizePercentage = 0.1f;
    [SerializeField] private float _timeUntilMaxCharge = 1;
    [SerializeField] private float _minScale = 0.15f;
    [SerializeField] private float _maxScale = 2f;
    public ProjectileState state = ProjectileState.Charging;
    // Start is called before the first frame update
    public PlayerPainter Player { get; set; }
    float _chargeTime = 0;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ProjectileState.Charging:
                float t = _chargeTime / _timeUntilMaxCharge;
                t = Mathf.Clamp(t, 0, 1);
                t = 1 - Mathf.Pow(1 - t, 3);
                _sizePercentage = Mathf.Lerp(_minScale, _maxScale, t);
                _chargeTime += Time.deltaTime;
                transform.localScale = new Vector3(_sizePercentage, _sizePercentage, _sizePercentage);
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
        transform.localScale = new Vector3(_minScale, _minScale, _minScale);
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
        if (other.gameObject == Player.gameObject || state == ProjectileState.Charging || !other.gameObject.CompareTag("ProjectileObstacle"))
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _colorRadius * _sizePercentage);

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
        Gizmos.DrawWireSphere(transform.position, _colorRadius * _sizePercentage);
    }
}
