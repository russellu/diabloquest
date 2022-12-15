
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;
using System.Collections.Generic;  

public class EnemyScript : MonoBehaviour, HittableInterface
{
    Camera mainCamera;
    RaycastHit hit;
    NavMeshAgent agent;
    Animator anim;
    AudioSource source;
    public AudioClip[] clips; // 0=att, 1=die, 2=hit, 3=idle, 4=satt
    public Transform hpbar; 
    CreatureStats stats = new CreatureStats(35, 5, 10, 10, 85);
    bool alreadyDead;
    bool aggro;
    Transform aggroTransform;

    SphereCollider proximitySphereCollider;
    BoxCollider weaponCollider;
    CapsuleCollider bodyCollider;

    List<EnemyScript> linkedMobs = new List<EnemyScript>(); 

    public DamageNumber hurt;

    Transform playerTransform;

    public LayerMask enemiesOnly; 

    void Start()
    {
        playerTransform = GameObject.Find("elm").GetComponent<Transform>(); 

        ZoneManager.zoneEnemies.Add(this); 

        proximitySphereCollider = GetComponentInChildren<SphereCollider>();
        weaponCollider = GetComponentInChildren<BoxCollider>();
        bodyCollider = GetComponentInChildren<CapsuleCollider>(); 

        source = GetComponent<AudioSource>(); 
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>(); 
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Physics.IgnoreLayerCollision(6, 7); 
    }

    void Update()
    {


        //if(wandering)
            //agent.destination = Ra

        anim.SetFloat("speed", agent.velocity.magnitude);

        if (aggro)
            agent.destination = aggroTransform.position;
      //  else
      //      agent.destination = walkDest; 
    }


    Vector3 GetAttackPosition(Vector3 attackLocation) // get location to stand in front of enemy to attack
    {
        Vector3 diff = attackLocation - transform.position;
        Vector3 offset = diff.normalized * 3;

        return attackLocation - offset;  
    }

  
    public void Aggro(Transform aggroTransform, bool recursiveAggro)
    {

        if (recursiveAggro)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, 40, transform.up,  Mathf.Infinity, enemiesOnly);

            if (hits != null)
            {
                foreach (RaycastHit hit in hits)
                {
                    Debug.Log(hit.transform.name); 
                    if (hit.transform.position != transform.position)
                        hit.transform.gameObject.GetComponent<EnemyScript>().Aggro(aggroTransform, false); 
                }
            
            }
        }

        this.aggroTransform = aggroTransform; 
        agent.destination = aggroTransform.position;
        aggro = true;   
    }

    public void AddLinkedMob(EnemyScript mob)
    {
        linkedMobs.Add(mob);
    }

    public void ResumeChase()
    {
        if (!anim.GetBool("dead"))
        {

            anim.SetBool("attack", false);
            agent.isStopped = false;
        }
    }
    
    public void Attack()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetBool("attack", true);
            agent.isStopped = true;
        }
    }

    void GetHit()
    { 
    
    }

    void OnFlinch()
    {
        anim.SetBool("hit", false);



    }

    void OnDeath()
    {
        alreadyDead = true;
        anim.SetBool("dead", true);
        source.clip = clips[1];
        agent.enabled = false;
        aggro = false;
        source.Play();

        proximitySphereCollider.enabled = false;
        weaponCollider.enabled = false;
        bodyCollider.enabled = false;


        playerTransform.gameObject.GetComponentInChildren<ThirdPersonController>().ApplyExp(stats.expValue); 


    }

    public void TakeDamage(float damage, Vector3 hitLocation)
    {

        if (!aggro)
            Aggro(playerTransform, true); 

        bool killingShot = stats.ApplyDamage(damage);

        float hpValue = ((float)stats.currentHp / (float)stats.maxHp) * 1.5f < 0 ?
            0 : ((float)stats.currentHp / (float)stats.maxHp) * 1.5f;

        hpbar.localScale = new Vector3(1, 0.25f, hpValue);

        DamageNumber damageNumber = hurt.Spawn(transform.position, damage);


        if (killingShot && !alreadyDead)
        {
            OnDeath();

        }
        else if (!alreadyDead)
        {
            source.clip = clips[2];
            source.Play();
        }
        
    }

    void AttackSound()
    {
        source.clip = clips[0];
        source.volume = 0.05f; 
        source.Play();
    }

    public void SetStartDestination(Vector3 startDest)
    {

        //agent.destination = walkDest; 
    }

    public void NotifyAttackTargetDied()
    {
        anim.SetBool("attack", false);
    }

    public float DamageDone()
    {
        return stats.GetDamage(); 
    }
}
