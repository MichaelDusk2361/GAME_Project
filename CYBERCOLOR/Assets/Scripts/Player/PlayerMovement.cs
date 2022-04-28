using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float SpeedMultiplier = 10f;

    [Header("Debugging Variables")]
    [SerializeField] private bool _stunned;
    [SerializeField] private Vector2 _movementInput;
    [SerializeField] private Vector3 _moveVector;
    [SerializeField] private Vector2 _rotateInput;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = 0;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3);
    }

    void FixedUpdate()
    {
        if (_stunned)
            return;

        _moveVector = new Vector3(_movementInput.x, 0, _movementInput.y) * Speed * SpeedMultiplier * Time.fixedDeltaTime;
        _rigidbody.velocity = _moveVector;

        // Prevents reset of rotation to vector 0, 0 (0 degrees angle)
        if(_rotateInput.x != 0f && _rotateInput.y != 0f)
        {
            float angle = Mathf.Atan2(_rotateInput.x, _rotateInput.y) * Mathf.Rad2Deg;
            _rigidbody.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Hole") && !_stunned)
        {
            _stunned = true;
            StartCoroutine(OnStun());
        }
    }

    IEnumerator OnStun()
    {
        yield return new WaitForSeconds(3f);
        _stunned = false;
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

    public void OnRotate(InputAction.CallbackContext context) => _rotateInput = context.ReadValue<Vector2>();
}
