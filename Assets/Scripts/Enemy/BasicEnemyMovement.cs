using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class BasicEnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb = null;
    public float movementSpeed;
    public bool followPlayerOnX;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = Vector2.down * movementSpeed;
    }

    private void Update()
    {
        if (followPlayerOnX)
        {
            if (gameObject.transform.position.x > player.transform.position.x + 0.05)
            {
                Vector2 followVec = Vector2.left * movementSpeed + Vector2.down * movementSpeed;
                rb.velocity = followVec;
            } else if (gameObject.transform.position.x < player.transform.position.x - 0.05)
            {
                Vector2 followVec = Vector2.right * movementSpeed + Vector2.down * movementSpeed;
                rb.velocity = followVec;
            } else
            {
                rb.velocity = Vector2.down * movementSpeed;
            }
        }
    }
}
