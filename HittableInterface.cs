using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HittableInterface 
{

    void TakeDamage(float damage, Vector3 hitLocation);
    float DamageDone(); 


}
