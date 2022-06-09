using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Start()
    {
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }


    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _decceleration = 5f;
    private bool _stunned = false;
    private bool _grounded = false;
    [SerializeField] private GameObject _groundCheckGameObject;

    private Vector2 _movementInput;
    private Vector2 _rotateInput;

    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private PlayerProjectileController _projectileController;
    [SerializeField] private AudioClip _dashClip;
    [SerializeField] private AudioClip _knockbackClip;
    [SerializeField] private float _dashForce = 10;
    [SerializeField] private float _dashCooldown = 2;
    private bool _dashOnCooldown = false;
    [SerializeField] private float _knockbackForce = 10;
    private AudioSource _audioSource;

    public bool Stunned
    {
        get { return _stunned; }
        set { _stunned = value; _rigidbody.isKinematic = _stunned; _projectileController.ReleaseProjectile(); }
    }

    public void BlinkWhileStunned(float stunDuration)
    {
        StartCoroutine(Blink(stunDuration));
    }

    IEnumerator Blink(float stunDuration)
    {
        float remainingStunTime = stunDuration;
        while (remainingStunTime > 0)
        {
            remainingStunTime -= 0.3f;
            yield return new WaitForSeconds(0.15f);
            _renderer.enabled = false;
            yield return new WaitForSeconds(0.15f);
            _renderer.enabled = true;
        }
    }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _projectileController = GetComponent<PlayerProjectileController>();
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _rigidbody.maxAngularVelocity = 0;
        VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        _grounded = Physics.Raycast(_groundCheckGameObject.transform.position, Vector3.down, 1);
        if (ShakeElapsedTime > 0)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

            ShakeElapsedTime -= Time.deltaTime;
        }
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
        _audioSource.PlayOneShot(_dashClip);
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
        ShakeElapsedTime = ShakeDuration;
        _audioSource.PlayOneShot(_knockbackClip);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(_knockbackForce * scale * new Vector3(forceDir.x, 0, forceDir.z).normalized, ForceMode.Impulse);
        StartCoroutine(StopShakeCamera());

    }

    IEnumerator StopShakeCamera()
    {
        yield return new WaitForSeconds(ShakeDuration);
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

    public void OnRotate(InputAction.CallbackContext context) => _rotateInput = context.ReadValue<Vector2>();
}
