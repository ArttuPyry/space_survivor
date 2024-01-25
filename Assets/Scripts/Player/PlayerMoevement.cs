using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoevement : MonoBehaviour
{
    private CustomInputs inputs = null;
    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D rb = null;
    private float moveSpeed = 1.0f;

    public SpriteRenderer shipSprite;
    public Sprite shipDefault;
    public Sprite shipTurn;
    public SpriteRenderer fireSprite;
    public Sprite fireDefault;
    public Sprite fireBoost;
    public Sprite fireLow;

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

        if (moveVector.x < 0)
        {
            shipSprite.sprite = shipTurn;
            shipSprite.flipX = false;
        } else if (moveVector.x > 0) {
            shipSprite.sprite = shipTurn;
            shipSprite.flipX = true;
        } else
        {
            shipSprite.sprite = shipDefault;
        }

        if (moveVector.y > 0)
        {
            fireSprite.sprite = fireBoost;
        } else if (moveVector.y < 0)
        {
            fireSprite.sprite = fireLow;
        } else
        {
            fireSprite.sprite = fireDefault;
        }
    }

    private void Update()
    {
        // Top and bottom game borders
        if (transform.position.y >= 1.5f) transform.position = new Vector2(transform.position.x, 1.5f);
        if (transform.position.y <= -1.5f) transform.position = new Vector2(transform.position.x, -1.5f);
        // Left and right game borders
        if (transform.position.x >= 0.8f) transform.position = new Vector2(0.8f, transform.position.y);
        if (transform.position.x <= -0.8f) transform.position = new Vector2(-0.8f, transform.position.y);
    }
}
