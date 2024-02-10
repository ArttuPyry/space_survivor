using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Stats")]
public class PerkStats : Perk
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float crtiMultiplier;
    [SerializeField] private float expMultiplier;
    [SerializeField] private float iFrames;
    [SerializeField] private int luck;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int shield;

    public override void Apply(GameObject player)
    {
        // Attack damage
        if (attackDamage != 0f)
        {
            player.GetComponent<PlayerBasicAttack>().damage += attackDamage;
        }

        // Attack speed
        if (attackSpeed != 0f)
        {
            float dividedAttackCD = attackSpeed / 10f;
            player.GetComponent<PlayerBasicAttack>().attackCoolDown -= dividedAttackCD;

            if (player.GetComponent<PlayerBasicAttack>().attackCoolDown <= 0.09f)
            {
                player.GetComponent<PlayerBasicAttack>().attackCoolDown = 0.1f;
            }
        }

        // Bullet speed
        if (bulletSpeed != 0f)
        {
            float dividedAttackCD = bulletSpeed / 10f;
            player.GetComponent<PlayerBasicAttack>().bulletSpeed -= dividedAttackCD;

            if (player.GetComponent<PlayerBasicAttack>().bulletSpeed > 5f)
            {
                player.GetComponent<PlayerBasicAttack>().bulletSpeed = 5;
            }
        }

        // Crit multiplier
        if (crtiMultiplier != 0f)
        {
            player.GetComponent<PlayerBasicAttack>().critMultiplier += crtiMultiplier;
        }

        // Experience multiplier
        if (expMultiplier != 0f)
        {
            player.GetComponent<Player>().expMultiplier += expMultiplier;
        }

        // iFrames aka damage immunity
        if (iFrames != 0f)
        {
            player.GetComponent<Player>().iFramesLengthInSec += iFrames;
        }

        // Luck
        if (luck != 0)
        {
            player.GetComponent<Player>().luck += luck;
        }

        // Movement Speed
        if (movementSpeed != 0f)
        {
            float dividedAttackCD = movementSpeed / 10f;
            player.GetComponent<PlayerMoevement>().moveSpeed += movementSpeed;
        }

        // Shield
        if (shield != 0)
        {
            player.GetComponent<Player>().currentShieldAmount += shield;
            player.GetComponent<Player>().UpdateShield();
        }
    }
}
