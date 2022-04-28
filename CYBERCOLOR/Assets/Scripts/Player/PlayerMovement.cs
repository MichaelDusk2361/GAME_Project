using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float SpeedMultiplier = 10f;

    [SerializeField] private Vector2 _movementInput;
    [SerializeField] private Vector3 _moveVector;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _moveVector = new Vector3(_movementInput.x, 0, _movementInput.y) * Speed * SpeedMultiplier * Time.fixedDeltaTime;
        //_rigidbody.AddForce(_moveVector, ForceMode.Acceleration);
        _rigidbody.velocity = _moveVector;
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();
}
