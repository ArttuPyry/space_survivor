using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularEnemyAttack : MonoBehaviour
{
    enum AttackType { Basic, Multi, Spiral }
    [Header("Attack type and base settings")]
    [SerializeField] private AttackType attackType;
    [SerializeField] private float attackCooldown;
    private float time;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject pooler;

    [Header("Basic attack settings")]
    [SerializeField] private GameObject[] shootSpots;
    [SerializeField] private GameObject[] aimSpots;
    [SerializeField] private bool aimAtPlayer;
    private GameObject player;

    [Header("Multishot settings")]
    [SerializeField] private int bulletAmount;
    [SerializeField] private float startAngle = 90f, endAngle = 270f;

    [Header("Spiral settings")]
    [SerializeField] private bool doubleSpiral;
    private float spiralAngle = 0f;
    [SerializeField] private float spiralAngleAdd = 10f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

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
            switch (attackType)
            {
                case AttackType.Basic:
                    for (int i = 0; i < shootSpots.Length; i++)
                    {
                        BasicAttack(shootSpots[i], aimSpots[i]);
                    }
                    break;
                case AttackType.Multi:
                    FireMultiShot();
                    break;
                case AttackType.Spiral:
                    SpiralAttack();
                    break;
                default:
                    break;
            }

        }
    }

    private void BasicAttack(GameObject shootSpot, GameObject aimSpot)
    {
        GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();

        if (aimAtPlayer && bullet != null)
        {
            bullet.transform.position = shootSpot.transform.position;
            Vector3 shootDir = (player.transform.position - shootSpot.transform.position).normalized;
            bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, 1);
            bullet.SetActive(true);
        } else
        {
            bullet.transform.position = shootSpot.transform.position;
            Vector3 shootDir = (aimSpot.transform.position - shootSpot.transform.position).normalized;
            bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, 1);
            bullet.SetActive(true);
        }
    }

    private void FireMultiShot()
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

    private void SpiralAttack()
    {

        if (!doubleSpiral)
        {
            float bulDirX = transform.position.x + Mathf.Sin((spiralAngle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((spiralAngle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<ModularBullet>().Setup(bulDir, bulletSpeed, 1);
            bullet.SetActive(true);
        } else
        {
            for (int i = 0; i <= 1; i++)
            {
                float bulDirX = transform.position.x + Mathf.Sin(((spiralAngle + 180f * i) * Mathf.PI) / 180f);
                float bulDirY = transform.position.y + Mathf.Cos(((spiralAngle + 180f * i) * Mathf.PI) / 180f);

                Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                Vector2 bulDir = (bulMoveVector - transform.position).normalized;

                GameObject bullet = pooler.GetComponent<ObjectPooler>().GetPooledObject();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<ModularBullet>().Setup(bulDir, bulletSpeed, 1);
                bullet.SetActive(true);
            }
        }


        spiralAngle += spiralAngleAdd;

        if (spiralAngle > 360f)
        {
            spiralAngle = 0f;
        }
    }
}
