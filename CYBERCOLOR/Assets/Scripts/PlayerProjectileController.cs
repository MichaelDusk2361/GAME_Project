using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    private ProjectileController _currentlyHeldProjectile;

    [SerializeField] private float _projectileCooldown = 0.5f;
    [SerializeField] private float _currentProjectileCooldown = 0f;

    public bool IsCharging { get => _currentlyHeldProjectile != null; }

    private PlayerMovement _playerMovement;
    private PlayerPainter _playerPainter;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerPainter = GetComponent<PlayerPainter>();

        _currentProjectileCooldown = _projectileCooldown;
    }

    private void Update()
    {
        if (_currentProjectileCooldown > 0)
            _currentProjectileCooldown -= Time.deltaTime;
    }

    public void OnProjectileUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ReleaseProjectile();
    }

    public void ReleaseProjectile()
    {
        if (_currentlyHeldProjectile != null)
        {
            _currentlyHeldProjectile.Release();
            _currentlyHeldProjectile = null;
            _currentProjectileCooldown = _projectileCooldown;
        }
    }

    public void OnProjectileDown(InputAction.CallbackContext context)
    {
        if (!context.performed || _currentProjectileCooldown > 0 || _playerMovement.Stunned) return;

        _currentlyHeldProjectile = Instantiate(_projectile, transform).GetComponent<ProjectileController>();
        _currentlyHeldProjectile.Init(_playerPainter);
    }
}
