
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class EnemyScript : MonoBehaviour, HittableInterface
{
    Camera mainCamera;
    RaycastHit hit;
    NavMeshAgent agent;
    Animator anim;
    AudioSource source;
    public AudioClip[] clips; // 0=att, 1=die, 2=hit, 3=idle, 4=satt
    public Transform hpbar; 
    CreatureStats stats = new CreatureStats(15, 5, 10, 10);
    bool alreadyDead;
    bool aggro;
    Transform aggroTransform;

    SphereCollider proximitySphereCollider;
    BoxCollider weaponCollider;
    CapsuleCollider bodyCollider;

    public DamageNumber hurt;




    void Start()
    {
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
        anim.SetFloat("speed", agent.velocity.magnitude); 

        if (aggro)
            agent.destination = aggroTransform.position; 
    }


    Vector3 GetAttackPosition(Vector3 attackLocation) // get location to stand in front of enemy to attack
    {
        Vector3 diff = attackLocation - transform.position;
        Vector3 offset = diff.normalized * 3;

        return attackLocation - offset;  
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            anim.SetBool("hit", true); 
        }
    }
    */
    public void Aggro(Transform aggroTransform)
    {
        this.aggroTransform = aggroTransform; 
        agent.destination = aggroTransform.position;
        aggro = true;   
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
        anim.SetBool("attack", true); 
        agent.isStopped = true;  
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

    }

    public void TakeDamage(float damage, Vector3 hitLocation)
    {

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

    public void NotifyAttackTargetDied()
    {
        anim.SetBool("attack", false);
        Debug.Log("stopping attak"); 
    }

    public float DamageDone()
    {
        return stats.GetDamage(); 
    }
}
