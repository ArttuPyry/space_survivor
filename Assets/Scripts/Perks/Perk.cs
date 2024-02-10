using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perk : ScriptableObject
{
    public string perkName;
    public string perkDescription;
    public Sprite perkSprite;
    public abstract void Apply(GameObject player);
}
