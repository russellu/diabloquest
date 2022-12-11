using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWeaponScript : MonoBehaviour
{

    HittableInterface owningCreature;

    // Start is called before the first frame update
    void Start()
    {
        owningCreature = GetComponentInParent<EnemyScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

           
            other.gameObject.GetComponent<ThirdPersonController>().TakeDamage(
                owningCreature.DamageDone(), transform.position);

        }
    }


}
