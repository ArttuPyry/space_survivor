using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int currentShieldAmount;
    public int maxShieldAmount;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void TakeHit()
    {
        if (currentShieldAmount > 0)
        {
            currentShieldAmount -= 1;
        } else
        {
            print("die");
        }
    }

}
