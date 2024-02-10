using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Damage")]
public class PerkAttackDamage : Perk
{
    public float attackDamageAmount;

    public override void Apply(GameObject player)
    {
        player.GetComponent<PlayerBasicAttack>().damage += attackDamageAmount;
    }
}
