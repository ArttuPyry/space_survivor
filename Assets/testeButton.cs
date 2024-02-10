using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testeButton : MonoBehaviour
{
    public Perk perk;
    public GameObject player;

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            AddPerk();
        }
    }

    public void AddPerk()
    {
        perk.Apply(player.gameObject);
    }
}
