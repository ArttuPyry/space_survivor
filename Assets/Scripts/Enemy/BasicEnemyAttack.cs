using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    public float attackCoolDown;
    private float time;

    public float bulletSpeed;

    public GameObject[] shootSpots;
    public GameObject pooler;

    private void Start()
    {
        time = attackCoolDown;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
        {
            time = attackCoolDown;

            for (int i = 0; i < shootSpots.Length; i++)
            {
                Attack(shootSpots[i]);
            }
        }
    }

    private void Attack(GameObject shootSpot)
    {
        GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = shootSpot.transform.position;
            bullet.GetComponent<EnemyBullet>().Setup(Vector3.down, bulletSpeed);
            bullet.SetActive(true);
        }
    }
}
