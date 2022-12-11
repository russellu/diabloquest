using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSensorScript : MonoBehaviour
{

    EnemyScript parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyScript>(); 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player trigger entered");
            parent.Aggro(other.transform); 
        }



    }

    
}
