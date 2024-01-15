using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 shootDir;
    private float speed;

    private Rigidbody2D rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector3 shootDir, float speed)
    {
        this.shootDir = shootDir;
        this.speed = speed;
    }

    private void Update()
    {
        rb.velocity = shootDir * speed;

        // if outside of map disable bullet
        if (transform.position.y <= -2f) gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeHit();
        }
    }
}
