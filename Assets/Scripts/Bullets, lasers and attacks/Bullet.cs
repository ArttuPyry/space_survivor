using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

public class Bullet : MonoBehaviour
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
