using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats
{

    public float maxHp;
    public float currentHp; 
    public float baseDamage;
    public float AC;
    public float THAC0;
    public float hpRegenPerSecond; 


    /*
     * 
     * THAC0: to hit armor class 0
     * uses 20-sided dice
     * if your character has a THAC0 of 10, and they have an AC of 0, you need to roll >= 10 to hit them
     * if your character has THACO 10, and enemy has AC
     * 
     */

    public CreatureStats(float maxHp, float baseDamage, float AC, float THAC0)
    {
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.baseDamage = baseDamage;
        this.AC = AC;
        this.THAC0 = THAC0;
        hpRegenPerSecond = 1; 
    }

    public bool ApplyDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
            return true;
        else return false; 
        
    
    }

    public float GetDamage()
    {
        return baseDamage; 
    
    }


}
