using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    // Baic bullet stats
    public float attackCoolDown;
    private float time;
    public float bulletSpeed;
    public float damage;
    public float critMultiplier = 1;
    [Range(0, 3)]
    public int bulletSize = 1;

    // Booler and shoot and aim spots
    [SerializeField] private GameObject[] shootSpots;
    [SerializeField] private GameObject[] aimSpots;
    [SerializeField] private GameObject pooler;

    [SerializeField] private Player player;

    // Bullet sprites
    [SerializeField] private Sprite[] basicBulletSprites;
    [SerializeField] private Sprite[] critBulletSprites;

    public List<Perk> acquiredCritPerks;
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
                SetBulletSizeAndCrit(true, bullet);

                // Activates crit perks x2 if double shot
                if (acquiredCritPerks.Count > 0 )
                {
                    for (int i = 0;i < acquiredCritPerks.Count;i++)
                    {
                        acquiredCritPerks[i].Apply(this.gameObject);
                    }
                }
            } else
            {
                SetBulletSizeAndCrit(false, bullet);
                bullet.GetComponent<ModularBullet>().Setup(shootDir, bulletSpeed, damage);
            }

            bullet.SetActive(true);
        }
    }

    private void SetBulletSizeAndCrit(bool isCrit, GameObject bullet)
    {
        // Sprite
        if (isCrit)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = critBulletSprites[bulletSize];
        } else
        {
            bullet.GetComponent<SpriteRenderer>().sprite = basicBulletSprites[bulletSize];
        }

        // Collider
        switch (bulletSize)
        {
            case 0:
                bullet.GetComponent<CircleCollider2D>().radius = 0.02f;
                break;
            case 1:
                bullet.GetComponent<CircleCollider2D>().radius = 0.03f;
                break;
            case 2:
                bullet.GetComponent<CircleCollider2D>().radius = 0.04f;
                break;
            case 3:
                bullet.GetComponent<CircleCollider2D>().radius = 0.05f;
                break;
        }
    }
}
