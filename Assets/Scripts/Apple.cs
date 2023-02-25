using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Enemy
{

    public enum AppleState { Wander, Chase, Search, Attack, PostAttackCheck, stunned, Death }
    public AppleState myAppleState;

    public int MyHp = 100;
    public bool SpottedPlayer;
    [SerializeField] List<Transform> navPoints = new List<Transform>();
    public Transform targetDestination, lastDestination;
    public bool ReachedNavPoint;
    public bool searching = false;
    int targetDir;
    float DistanceToTarget;
    // Start is called before the first frame update
    [SerializeField] float MysightRadius;
    bool chasingVictim;
    public int lastKnowVictimDir;

    SpriteRenderer mySpriteRenderer;
    [SerializeField] float AttackRange;
    [SerializeField] Transform rotationHousing;
    Animator myAnim;
    float attentionSpan = 3;
    float searchTimer;
    [SerializeField] ParticleSystem myDeathFX;
    [SerializeField] float knockbackForce;
    bool stunned;
    public AudioSource myAudioSource;
    [SerializeField] List<AudioClip> mySoundFX = new List<AudioClip>();
    bool isdead = false;
    [SerializeField] float stunnedtimer;
   public  Vector2 lastKnownVictimLocation;
    [SerializeField]GameObject victimRef;
   public bool isMoving;
    [SerializeField] float maxSpeed;
    public void OnDrawGizmos()
    {
        base.OnDrawGizmos(MysightRadius);
    }
    void Start()
    {
        MyHp = Random.Range(1, 30);
        SetScaleBasedOnHP();
        SetRandomWanderDestination();

        initializeAndSet();
        Rotsputen.OnBossdeath += KillSelf;

    }

    private void SetScaleBasedOnHP()
    {
        if (MyHp <= 10)
        {
            transform.localScale = new Vector2(.85f, 85f);

        }

        else if (MyHp <= 20 && MyHp > 10)
        {

            transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (MyHp <= 30 && MyHp > 20)
        {
            transform.localScale = new Vector2(1.8f, 1.8f);
        }
    }

    private void initializeAndSet()
    {
        float appleScale = Random.Range(.9f, 1.1f);
        transform.localScale = new Vector2(appleScale, appleScale);
        myAudioSource = GetComponent<AudioSource>();
        myAnim = GetComponentInChildren<Animator>();
        initializer();

        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SpottedPlayer = LookForPlayer(MysightRadius); // draw sight radius to check if play is visible 

        if (victim != null)
        {
            victimRef = victim.gameObject;

        }

        switch (myAppleState)
        {
            case AppleState.Wander:
                // search for navPoint
                // Wander(targetDestination);
              
               
                    MovetoDestination();
                    ReachedDesinationCheck();
               

                
                break;

            case AppleState.Chase:
               
                //spotted set target and get close



                MovetoDestination();
                SetupChaseDependencies();
           
                break;

            case AppleState.Search:

                // target out of sight still looking in last known direction 

                SearchForLostVictim();
                MovetoDestination();
                break;

            case AppleState.Attack:

                // when close enough
                isMoving = false;
                myAnim.SetBool("Attack", true);
                
                break;

            case AppleState.PostAttackCheck:

                // when close enough
                isMoving = false;
                myAnim.SetBool("Attack", false);
                ContinueAttackCheck();

                // are we in range for another attack?
                break;



         

            case AppleState.stunned:
                // chill state 

                break;







        }




        CalculateDirectionAndDistance();




        //ReachedDestination();

        Death();



    }
    private void FixedUpdate()
    {
        if(isMoving)
        {
        movespeedCalculation();

        }
    }

    /// <summary>
    /// wander to random positions and look for player/ victims if edible NPCs are implemented
    /// </summary>
    /// <param name="destination"></param>
    public override void Wander(Transform destination)
    {


       
      


      
    }

    public void MovetoDestination()
    {
        

        isMoving = true;
        if (SpottedPlayer)
        {
            victimRef = victim.gameObject;
            targetDestination = victimRef.transform;

            myAppleState = AppleState.Chase;
        }
    }


    private void CalculateDirectionAndDistance()
    {
        rotationHousing.eulerAngles = DistanceToTarget > 0 ? new Vector2(0, 0) : new Vector2(0, 180);
         targetDir = DistanceToTarget > 0 ? 1 : -1;
        DistanceToTarget = targetDestination.position.x - transform.position.x;
       // targetDir = DistanceToTarget > 0 ? 1 : -1;
    }

    /// <summary>
    ///  did i spot player/ victim? chase.  
    ///  did i lose sight of the player/ victim? search last known direction
    /// </summary>
    private void SetupChaseDependencies()     
    {
        targetDestination = victimRef.transform;

        if (SpottedPlayer)
        {
            
           
            searchTimer = 0; // reset attention to chase victim

        }
        if (!SpottedPlayer)
        {
            
            // store last known Position
            myAppleState = AppleState.Search;

            SearchForLostVictim();

            //look for duration in recent direction
            // while timer goes move in the last know direction unless run into a wall then look to jump location else return to wandering. 

        }
        ReachedDesinationCheck();
    }
    void ReachedDesinationCheck()
    {
        ReachedNavPoint = Mathf.Abs(DistanceToTarget) <= AttackRange ? true : false;
        if (ReachedNavPoint)
        {

          if(SpottedPlayer)
          {
                myAppleState = AppleState.Attack;
          }
          else
          {
                SetRandomWanderDestination();
                

          }
                
        }
    }
    
 
    void ContinueAttackCheck()
    { 
     if(SpottedPlayer)
        {
            myAppleState = AppleState.Chase;
        }
     else 
        {
            myAppleState = AppleState.Search;
           
        }
    
    }

    private void SetRandomWanderDestination()
    {
       targetDestination = navPoints[Random.Range(0, navPoints.Count)];
        ReachedNavPoint = false;

        myAppleState = AppleState.Wander;
    }

    public override void TakeDamage(int damageValue)
    {
        MyHp -= damageValue;
        myRigidbody.AddForce(Vector2.right * -targetDir * knockbackForce, ForceMode2D.Impulse);
        myAnim.SetTrigger("Startstunned");
        StartCoroutine(HitStun(stunnedtimer));

    }
    IEnumerator HitStun(float stunnedtimer)
    {

        stunned = true;
        yield return new WaitForSeconds(stunnedtimer);
        stunned = false;
        myAnim.SetTrigger("stunnEnd");

    }
    void KillSelf()
    {
        StartCoroutine(DelayDeath());
    }
    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(2f);
        MyHp = 0;
    }
    public override void Death()
    {
        if (MyHp <= 0 && !isdead)
        {
            myAppleState = AppleState.Death;
            myAudioSource.pitch = Random.Range(.9f, 1.1f);
            myAudioSource.PlayOneShot(mySoundFX[0]);
            myDeathFX.Play();
            GameManager.instance.AwardPlayerPoints(150, transform.position);
            myDeathFX.transform.parent = null;
            Destroy(myDeathFX, 2);
            Collider2D mycollider = GetComponent<Collider2D>();
            mycollider.enabled = false;
            mySpriteRenderer.enabled = false;
            isdead = true;
        }
    }


  
   
    public override bool LookForPlayer(float sightRadius)
    {
        
        return base.LookForPlayer(sightRadius);
    }
   

   
    /// <summary>
    /// search for lost victim attention span is based on a search timer, longer search timer the longer it will look for player/victim
    /// </summary>
    public override void SearchForLostVictim()
    {
     
       


        searchTimer += Time.deltaTime;
      if(searchTimer< attentionSpan) // for player for duration of apples attention span
        {
            searching = true;
           

        }
        if (searchTimer > attentionSpan) // im bored think i lost him, im  gonna wander
        {
            searching = false;
            SetRandomWanderDestination();

        }


        if (SpottedPlayer)
        {
            myAppleState = AppleState.Chase;
        }
      
       
     
    }

    private void movespeedCalculation()
    {
       
        myRigidbody.AddForce(Vector2.right * targetDir * moveSpeed, ForceMode2D.Force);
        myRigidbody.velocity = new Vector2(Mathf.Clamp(myRigidbody.velocity.x, -maxSpeed, maxSpeed), myRigidbody.velocity.y);

      
    }

    public void LungeForward()
    {
        myRigidbody.AddForce(Vector2.right * targetDir * moveSpeed*2f, ForceMode2D.Impulse);
       

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==12)
        knockbackForce = 12;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        knockbackForce = 5;
    }
    public void SetPostAttackState()
    {
        myAppleState = AppleState.PostAttackCheck;
    }
}
