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

    [SerializeField] private float _outOfBoundsXZ = 20;
    public PlayerPainter Player { get; set; }
    float _chargeTime = 0;

    [SerializeField] private GameObject _onHitVFX;
    [SerializeField] private ParticleSystem _chargingVFX;

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > _outOfBoundsXZ ||
            Mathf.Abs(transform.position.z) > _outOfBoundsXZ)
            Explode();

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
        GetComponent<MeshRenderer>().material = player.PaintMaterial;
        var settings = _chargingVFX.main;
        settings.startColor = player.PaintMaterial.color;
        _chargingVFX.Play();
    }
    IEnumerator Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        state = ProjectileState.Charging;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public void Release()
    {
        state = ProjectileState.Released;
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        _chargingVFX.Stop();
        _chargingVFX.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Player == null || other.gameObject == Player.gameObject || state == ProjectileState.Charging || !(other.gameObject.CompareTag("ProjectileObstacle") || other.gameObject.CompareTag("Player")))
            return;

        Explode();
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _colorRadius * _sizePercentage);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<PaintableFloor>() is PaintableFloor paintableFloor)
            {
                paintableFloor.PaintFloor(Player);
            }

            if (collider.gameObject.GetComponent<PlayerMovement>() is PlayerMovement otherPlayer)
            {
                if (otherPlayer.gameObject != Player.gameObject)
                    otherPlayer.Knockback(otherPlayer.transform.position - transform.position, _sizePercentage);
            }
        }

        // Spawn OnHitVFX
        var vfx = Instantiate(_onHitVFX, transform.position, _onHitVFX.transform.rotation);
        var settings = vfx.GetComponent<ParticleSystem>().main;
        settings.startColor = Player.PaintMaterial.color;

        Destroy(vfx, 2f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _colorRadius * _sizePercentage);
    }
}
