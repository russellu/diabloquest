using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 *  static methods for managing zone 
 *  example: when player dies, all attacking creatures need to be notified
 */
public static class ZoneManager 
{

    public static List<EnemyScript> zoneEnemies = new List<EnemyScript>();

    public static void NotifyPlayerDeath()
    {
        foreach (EnemyScript enemy in zoneEnemies)
            enemy.NotifyAttackTargetDied(); 
    
    }

      
}
