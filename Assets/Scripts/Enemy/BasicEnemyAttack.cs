using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    private float time;

    [SerializeField] private float bulletSpeed;

    [SerializeField] private GameObject[] shootSpots;
    [SerializeField] private GameObject[] aimSpots;
    [SerializeField] private GameObject pooler;

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
                Attack(shootSpots[i], aimSpots[i]);
            }
        }
    }

    private void Attack(GameObject shootSpot, GameObject aimSpot)
    {
        GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = shootSpot.transform.position;
            Vector3 shootDir = (aimSpot.transform.position - shootSpot.transform.position).normalized;
            bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, 1);
            bullet.SetActive(true);
        }
    }
}
