using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    [SerializeField] private bool playersAttack;

    [SerializeField] private bool splittingBullet;
    [SerializeField] private int splitAmount;
    [SerializeField] private float startAngle = 90f, endAngle = 270f;
    private GameObject pooler;

    private Vector3 shootDir;
    private float speed;
    private float damage;

    private Rigidbody2D rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pooler = GameObject.FindGameObjectWithTag("EnemyBulletPooler");
    }

    public void Setup(Vector3 shootDir, float speed, float damage)
    {
        this.shootDir = shootDir;
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        rb.velocity = shootDir * speed;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
        {
            if (splittingBullet)
            {
                Split();
                gameObject.SetActive(false);
            }
        }

        // if outside of map disable bullet
        if (transform.position.y >= 2f) gameObject.SetActive(false);
        if (transform.position.y <= -2f) gameObject.SetActive(false);
        if (transform.position.x >= 1.5f) gameObject.SetActive(false);
        if (transform.position.x <= -1.5f) gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        if (playersAttack && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            gameObject.SetActive(false);
        }

        if (!playersAttack && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeHit();
        }
    }

    private void Split()
    {
        float angleStep = (endAngle - startAngle) / splitAmount;
        float angle = startAngle;

        for (int i = 0; i < splitAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<ModularBullet>().Setup(bulDir, speed, 1);
            bullet.SetActive(true);

            angle += angleStep;
        }
    }
}
