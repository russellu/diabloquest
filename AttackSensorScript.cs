using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensorScript : MonoBehaviour
{

    EnemyScript parent; 

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<EnemyScript>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.Attack();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            parent.ResumeChase();
    }


}
