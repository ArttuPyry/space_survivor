using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    public float attackCoolDown;
    private float time;

    public float attackSpeed;

    public GameObject[] shootSpots;

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
        GameObject bullet = ObjectPooler.instance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = shootSpot.transform.position;
            bullet.GetComponent<Bullet>().Setup(Vector3.up, attackSpeed);
            bullet.SetActive(true);
        }
    }
}
