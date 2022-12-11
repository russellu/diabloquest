using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    HittableInterface owningCreature;
    AudioSource source;

    public GameObject bloodSplat; 


    private void Start()
    {
        source = GetComponent<AudioSource>(); 
        owningCreature = GetComponentInParent<ThirdPersonController>(); 
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.tag == "orc")
        {

            ParticleSystem currentSplat = Instantiate(bloodSplat).GetComponent<ParticleSystem>();
            currentSplat.transform.position = transform.position;
            currentSplat.startColor = new Color(0.1f, .8f, 0); 
            currentSplat.Play(); 

            Debug.Log("hit an orc"); 
            source.Play(); 
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(owningCreature.DamageDone(), transform.position); 
        }
    }


}
