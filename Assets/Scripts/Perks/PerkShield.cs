using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Shield")]
public class PerkShield : Perk
{
    public int shieldAmount;
    public override void Apply(GameObject player)
    {
        player.GetComponent<Player>().currentShieldAmount += shieldAmount;
        player.GetComponent<Player>().UpdateShield();
    }
}
