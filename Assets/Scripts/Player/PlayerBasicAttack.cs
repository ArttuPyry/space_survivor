using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    public float attackCoolDown;
    private float time;

    public float bulletSpeed;
    public float damage;

    public float critMultiplier = 1;

    [SerializeField] private GameObject[] shootSpots;
    [SerializeField] private GameObject[] aimSpots;
    [SerializeField] private GameObject pooler;

    [SerializeField] private Player player;

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
            int randomIndex = Random.Range(1, 100);

            bullet.transform.position = shootSpot.transform.position;
            Vector3 shootDir = (aimSpot.transform.position - shootSpot.transform.position).normalized;

            if (randomIndex <= player.luck)
            {
                bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, damage * critMultiplier);
            } else
            {
                bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, damage);
            }

            bullet.SetActive(true);
        }
    }
}
