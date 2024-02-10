using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/AttackSpeed")]
public class PerkAttackSpeed : Perk
{
    public float attackSpeedAmount;
    public float attackDamageAmount;

    public override void Apply(GameObject player)
    {
        float dividedAttackCD = attackSpeedAmount / 10f;
        player.GetComponent<PlayerBasicAttack>().attackCoolDown -= dividedAttackCD;

        if ((player.GetComponent<PlayerBasicAttack>().attackCoolDown) <= 0.09f)
        {
            player.GetComponent<PlayerBasicAttack>().attackCoolDown = 0.1f;
        }
    }
}
