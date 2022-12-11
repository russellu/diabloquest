using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DamageNumbersPro;
using UnityEngine.Animations.Rigging; 

public class ThirdPersonController : MonoBehaviour, HittableInterface
{

    Animator anim;
    CharacterController controller; 
    public Transform groundCheck;
    public LayerMask mask;
    public Transform cameraTransform;
    AudioSource source;
    Vector3 velocity;
    float maxSpeed = 15; 
    float gravity = -9.81f / 8f; 
    public AudioClip[] clips;
    public Image image; 
    CreatureStats stats = new CreatureStats(100, 10, 10, 10);
    public Transform parentTransform;
    bool attackAnimation;
    public DamageNumber hurt;
    public DamageNumber regen; 
    float flinchSum = 0;

    float sitDamageMultiplier = 1; 

    BoxCollider weaponCollider;
    CapsuleCollider bodyCollider;

    public Rig rig;
    public Transform leftHandEffector;
    public Transform rightHandEffector;
    public Transform guardPoint;

    public GameObject blockParticles;
    public GameObject bloodSplat;

    bool blocking = false;
    float hpRegenSum = 0;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public Transform bow;
    //Transform fletchPoint;
    public LineRenderer shotPathRenderer; 
    bool bowLocked;
    public GameObject arrow;
    public Transform chest; 

    GameObject currentArrow;

    Quaternion savedChestRotation; 

 
    // Start is called before the first frame update
    void Start()
    {

        shotPathRenderer = Instantiate(shotPathRenderer);
        shotPathRenderer.enabled = false; 

        weaponCollider = GetComponentInChildren<BoxCollider>();
        weaponCollider.enabled = false;

        bodyCollider = GetComponentInChildren<CapsuleCollider>(); 

        controller = GetComponentInParent<CharacterController>(); 
        anim = GetComponent<Animator>();

        source = GetComponent<AudioSource>();

        guardPoint.position = transform.position + Vector3.forward; 

    }


 

    // Update is called once per frame
    void Update()
    {

        if (anim.GetBool("dead"))
            return;
            
        if (Input.GetAxis("Fire3") > 0)
        {
            anim.SetBool("range_attack", true);
            if (bowLocked)
                anim.speed = 0f;
            
        }
        else if (bowLocked)
        {
            chest.rotation = Quaternion.Euler(0, -90, 0);  
            bowLocked = false;
            currentArrow.GetComponent<ArrowScript>().InitiateFlight(currentArrow.transform.position,
                currentArrow.transform.position + transform.right * 100);

            shotPathRenderer.enabled = false; 
            source.clip = clips[7];
            source.volume = 2; 
            source.Play();
            anim.speed = 1f;
            anim.SetBool("range_attack", false);
            rig.weight = 0; 
        }

        if (anim.GetBool("sit") && stats.currentHp < stats.maxHp)
        {
            hpRegenSum += Time.deltaTime * stats.hpRegenPerSecond;
            if (hpRegenSum >= 2)
            {
                hpRegenSum = 0;
                DamageNumber damageNumber = regen.Spawn(transform.position, 2);   
                stats.currentHp += 2; 
                image.GetComponent<RectTransform>().localScale = new Vector3(stats.currentHp / stats.maxHp, 1, 1);
            }
        }

        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 100;
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime;

        if (moveY < 0)
            moveY = moveY * maxSpeed / 2;
        else 
            moveY = moveY * maxSpeed; 

        if (moveX != 0)
            anim.SetFloat("rot", 1);
        else 
            anim.SetFloat("rot", 0);

        velocity.y += gravity * Time.deltaTime; 
        
        Vector3 moveForward = moveY * parentTransform.forward;
        Vector3 finalMove = moveForward + velocity;

        if (!bowLocked)
        {
            controller.Move(finalMove);
            parentTransform.Rotate(new Vector3(0, moveX, 0));
        }
        else // bow is locked, adjust aim with movement instead
        {
            shotPathRenderer.enabled = true;

            rig.weight = 1;

            Vector3[] positions = { currentArrow.transform.position,
                currentArrow.transform.position - currentArrow.transform.up*100 };

            shotPathRenderer.SetPositions(positions);

            chest.Rotate(transform.right, moveY*3);
            chest.Rotate(transform.up, moveX*2);

            
        }


        float jump = Input.GetAxis("Jump");
        if (jump > 0)
        {
            anim.SetBool("jump", true);
        }
        else 
            anim.SetBool("jump", false); 

        bool grounded = Physics.CheckSphere(groundCheck.position, 1, mask);

        if (!grounded)    
            controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));

        float rev = moveY < 0 ? -1 : 1; 

        anim.SetFloat("speed", controller.velocity.magnitude * rev);

        if (Input.GetAxis("Fire1") > 0 && Input.GetAxis("Fire2") == 0)
        {
            anim.SetBool("attack", true);
        }
        else 
            anim.SetBool("attack", false);


        //if (rig.weight > 0)
        //    rig.weight -= Time.deltaTime * 4;

        if (Input.GetAxis("Fire2") > 0 && !attackAnimation)
        {
            blocking = true;
            guardPoint.position = (transform.position + parentTransform.forward*1.5f);
            leftHandEffector.position = guardPoint.position;
            rightHandEffector.position = guardPoint.position;
            leftHandEffector.rotation = guardPoint.rotation;
            rightHandEffector.rotation = guardPoint.rotation;
            rig.weight = 1;
        }
        else blocking = false; 


        HandleCombatLayers(); 


    }

    void HandleCombatLayers()
    {


        if (attackAnimation)
        {
            anim.SetLayerWeight(1, 1);
            maxSpeed = 7.5f;
        }
        else
        {
            maxSpeed = 15f;
            anim.SetLayerWeight(1, 0);
        }

        flinchSum -= Time.deltaTime; // decays to zero over 1 second

        if (flinchSum < 0)
            flinchSum = 0;

        anim.SetLayerWeight(2, flinchSum);

    }

    void Step()
    {
        source.volume = 0.05f; 
        source.clip = clips[0]; 
        source.Play(); 
    
    }

    void Jump()
    {
        velocity.y = 0.29f;

    }

    void JumpSound()
    {
        source.volume = 0.5f;
        source.clip = clips[1]; 
        source.Play();

    }

    void Whoosh()
    {
        source.volume = 1f; 
        source.clip = clips[2];
        source.Play(); 
    }


    void LockBow()
    {
      
        bowLocked = true;
        currentArrow = Instantiate(arrow);

        currentArrow.transform.parent = bow.transform;
        currentArrow.transform.localRotation = Quaternion.Euler(156, -21, 191);
        currentArrow.transform.localPosition = new Vector3(1.17f, 2.4f, -0.88f);
    }



    void BowLoad()
    {
        source.volume = 2; 
        source.clip = clips[6];
        source.Play(); 
    
    }

    void OnFlinch()
    {

    }

    void EnableWeapon()
    {
        weaponCollider.enabled = true;

    }

    void DisableWeapon()
    {
        weaponCollider.enabled = false;

    }

    void BlockEffect(Vector3 blockLocation)
    {
        source.clip = clips[5];
        source.Play();

        ParticleSystem block = Instantiate(blockParticles).GetComponent<ParticleSystem>();
        block.transform.position = transform.position;
        block.Play();


    }

    void DamageEffect(Vector3 damageLocation)
    {
        ParticleSystem currentSplat = Instantiate(bloodSplat).GetComponent<ParticleSystem>();
        currentSplat.transform.position = transform.position;
        currentSplat.Play();

    }

    public void TakeDamage(float damage, Vector3 hitLocation)
    {

        if (blocking)
        {

            BlockEffect(hitLocation); 

            guardPoint.position = hitLocation; 
            leftHandEffector.position = guardPoint.position;
            rightHandEffector.position = guardPoint.position;
            leftHandEffector.rotation = guardPoint.rotation;
            rightHandEffector.rotation = guardPoint.rotation;
            rig.weight = 1;

        }
        else
        {
            damage = damage * sitDamageMultiplier; 

            DamageEffect(hitLocation); 

            DamageNumber damageNumber = hurt.Spawn(transform.position, damage);

            float damagePercHp = damage / stats.currentHp;

            if (damagePercHp < 0.25)
                damagePercHp = 0.25f; // cap it at 0.15

            flinchSum += damagePercHp;

            attackAnimation = false;
            bool dead = stats.ApplyDamage(damage);

            image.GetComponent<RectTransform>().localScale = new Vector3(stats.currentHp / stats.maxHp, 1, 1);

            if (dead)
            {
                OnDeath();
            }
            else
            {
                source.volume = 0.5f;
                source.clip = clips[3];
                source.Play();
            }
        }
    }

    void OnDeath()
    {
        ZoneManager.NotifyPlayerDeath();
        anim.SetLayerWeight(2, 0);
        source.volume = 0.5f;
        source.clip = clips[4];
        source.Play();
        anim.SetBool("dead", true);
        weaponCollider.enabled = false;
        bodyCollider.enabled = false; 
    }

    public float DamageDone()
    {
        return stats.GetDamage(); 
    }


    void StartAttack()
    {
        attackAnimation = true; 
    }

    void EndAttack()
    {
        attackAnimation = false; 
    
    }

    void SitUp()
    {
        anim.SetBool("sit", false);
        sitDamageMultiplier = 1; 
    }

    void SitDown()
    {

        sitDamageMultiplier = 2; 

    }
}
