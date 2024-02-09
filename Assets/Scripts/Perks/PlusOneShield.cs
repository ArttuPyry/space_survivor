using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Perks/Shield")]
public class PlusOneShield : Perk
{
    public override void Apply(GameObject player)
    {
        player.GetComponent<Player>().currentShieldAmount += 1;
        player.GetComponent<Player>().ShieldTest();
    }
}
