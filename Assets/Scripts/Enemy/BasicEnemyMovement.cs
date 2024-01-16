using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class BasicEnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb = null;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool followPlayerOnX;
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
        // If outside of map disable enemy
        if (transform.position.y <= -2f) gameObject.SetActive(false);

        // Follow player on X axis, pls follow me too https://twitter.com/orc_hugs
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeHit();
        }
    }
}
