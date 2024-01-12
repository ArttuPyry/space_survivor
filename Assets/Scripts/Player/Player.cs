using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private CustomInputs inputs = null;
    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D rb = null;
    private float moveSpeed = 1.5f;

    private void Awake()
    {
        inputs = new CustomInputs();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Movement.performed += OnMovementPerformed;
        inputs.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Player.Movement.performed -= OnMovementPerformed;
        inputs.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * moveSpeed;
    }
}
