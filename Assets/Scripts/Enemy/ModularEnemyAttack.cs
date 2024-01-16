using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private float time;

    [SerializeField] private float bulletSpeed;

    [SerializeField] private GameObject[] shootSpots;
    [SerializeField] private GameObject[] aimSpots;
    [SerializeField] private GameObject pooler;

    [SerializeField] private bool multiShot;
    [SerializeField] private int bulletAmount;
    [SerializeField] private float startAngle = 90f, endAngle = 270f;

    private void Start()
    {
        time = attackCooldown;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
        {
            time = attackCooldown;

            Fire();
        }
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<ModularBullet>().Setup(bulDir, bulletSpeed, 1);
            bullet.SetActive(true);

            angle += angleStep;
        }
    }

}
