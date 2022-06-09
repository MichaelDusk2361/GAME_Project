using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _decceleration = 5f;
    private bool _stunned = false;
    private bool _grounded = false;
    [SerializeField] private GameObject _groundCheckGameObject;

    private Vector2 _movementInput;
    private Vector2 _rotateInput;

    private Rigidbody _rigidbody;
    private PlayerProjectileController _projectileController;
    [SerializeField] private float _dashForce = 10;
    [SerializeField] private float _dashCooldown = 2;
    private bool _dashOnCooldown = false;
    [SerializeField] private float _knockbackForce = 10;

    public bool Stunned
    {
        get { return _stunned; }
        set { _stunned = value; _projectileController.ReleaseProjectile(); }
    }

    void Awake()
    {
        _projectileController = GetComponent<PlayerProjectileController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = 0;
    }

    private void Update()
    {
        _grounded = Physics.Raycast(_groundCheckGameObject.transform.position, Vector3.down, 1);
    }

    void FixedUpdate()
    {
        if (Stunned)
            return;

        if (_grounded)
        {
            // some big math
            Vector3 movementInput = Vector3.ClampMagnitude(new Vector3(_movementInput.x, 0, _movementInput.y), 1);
            float maxMovementSpeed = (_projectileController.IsCharging ? _maxSpeed * 0.4f : _maxSpeed);
            Vector3 targetSpeed = movementInput * maxMovementSpeed;
            Vector3 speedDif = targetSpeed - _rigidbody.velocity;
            float accelerationRate = targetSpeed.magnitude > 0.01f ? _acceleration : _decceleration;
            Vector3 movement = speedDif.magnitude * accelerationRate * speedDif.normalized;
            _rigidbody.AddForce(new Vector3(movement.x, 0, movement.z));
        }
        // Prevents reset of rotation to vector 0, 0 (0 degrees angle)
        if (_rotateInput.x != 0f || _rotateInput.y != 0f)
        {
            float angle = Mathf.Atan2(_rotateInput.x, _rotateInput.y) * Mathf.Rad2Deg;
            _rigidbody.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void OnDashDown(InputAction.CallbackContext context)
    {
        if (!context.performed || _dashOnCooldown || _stunned) return;

        StartCoroutine(DashCoolDown());
        _rigidbody.AddForce(new Vector3(_movementInput.x, 0, _movementInput.y).normalized * _dashForce, ForceMode.Impulse);
    }

    IEnumerator DashCoolDown()
    {
        _dashOnCooldown = true;
        yield return new WaitForSeconds(_dashCooldown);
        _dashOnCooldown = false;
    }

    public void Knockback(Vector3 forceDir, float scale)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(_knockbackForce * scale * new Vector3(forceDir.x, 0.1f, forceDir.z).normalized, ForceMode.Impulse);
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

    public void OnRotate(InputAction.CallbackContext context) => _rotateInput = context.ReadValue<Vector2>();
}
