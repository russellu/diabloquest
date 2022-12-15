using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats
{

    public float maxHp;
    public float currentHp;

    public float maxStam;
    public float currentStam;

    public float maxMana;
    public float currentMana;

    public float nextLvlExp;
    public float currentExp;
    public int level;
    public float[] levelExpThresholds = { 0, 2250, 4500, 9000, 18000, 36000, 75000, 
        150000, 300000, 600000, 900000, 1200000, 1500000, 1800000, 2100000, 2400000,
        2700000,3000000,3300000,3600000,3900000, 4200000, 4500000, 4800000, 5100000};
    // hobgoblin gives 35 exp

    public float baseDamage;
    public float AC;
    public float THAC0;
    public float hpRegenPerSecond;

    public float expValue; 


    /*
     * 
     * THAC0: to hit armor class 0
     * uses 20-sided dice
     * if your character has a THAC0 of 10, and they have an AC of 0, you need to roll >= 10 to hit them
     * if your character has THACO 10, and enemy has AC
     * 
     */

    public CreatureStats(float maxHp, float baseDamage, float AC, float THAC0, float expValue)
    {
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.baseDamage = baseDamage;
        this.AC = AC;
        this.THAC0 = THAC0;
        this.expValue = expValue; 
        hpRegenPerSecond = 1;

        maxStam = 100;
        currentStam = 100;

        maxMana = 100;
        currentMana = 100;

        nextLvlExp = 250;
        currentExp = 0;

        level = 1; 
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

    public void ReduceStamina(float amount)
    {
        currentStam -= amount;
        if (currentStam < 0)
            currentStam = 0; 
    }

    public void IncreaseStamina(float amount)
    {
        currentStam += amount;
        if (currentStam > maxStam)
            currentStam = maxStam;  
    }

    public void AddExp(float amount)
    {

        currentExp += amount;
        if (currentExp > levelExpThresholds[level])
            LevelUp(); 
    
    }


    public void LevelUp()
    {
        level++; 
        
    }

}
