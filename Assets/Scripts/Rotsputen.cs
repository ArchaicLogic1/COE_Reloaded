using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Rotsputen : Hackable
{
    public enum BossState { SelectDestination, Move, Attack, SpellCast, Idle, Death, WaitForPlayer,PostSpellWait }
    public BossState rotsputenState;
    [SerializeField] Animator myAnim;
    [SerializeField] float velocityY;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCheckRadius = .1f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Rigidbody2D myRigidBody;
    [SerializeField] Vector2 TargetDestination;
    [SerializeField] Transform[] NavPts;
    [SerializeField] float moveSpeed, maxSpeed;
    [SerializeField] List<int> spells = new List<int>();
    [SerializeField] int lastSpell, currentSpell;
    [SerializeField] GameObject spawnOfTheRot, RotSpawner, orbOfDeath;
    SpriteRenderer mySpriteRenderer;
    [SerializeField] Transform handPosition;
    [SerializeField] float orbForce;
    GameObject player;

    [SerializeField] Transform rotationHandler;
    float spellcastCooldown = 50;
    bool startCooldownTimer;
    [SerializeField] Transform ObstacleCheckTransform;
    [SerializeField] LayerMask obstacles;
    [SerializeField] float jumpForce;
    bool ismoving;
    RaycastHit2D raycastHit;
    [SerializeField] ParticleSystem jumpFx;
    [SerializeField] Image healthBar, healthbarDamageTell;
    [SerializeField] float Health, maxHealth, healthBarDamping;
    [SerializeField] Dialogue myDialogue;
    bool dialoguestarted;
    int takeDamageCounter;
    bool attackAnyway;
    [SerializeField] GameObject enemySpawner;
    Collider2D wtfAmIHitting;
    public delegate void BossDeath();
    public static event BossDeath OnBossdeath;
    public delegate void BossBattleStart();
    public static BossBattleStart onBossBattleStart;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Health;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("PlayerController");
        rotsputenState = BossState.WaitForPlayer;
        // rotsputenState = BossState.SelectDestination;
    }

    // Update is called once per frame
    void Update()
    {
        GroundedCheck();

        BossStateUpdate();

        SpellCooldownTimer();
        if (healthbarDamageTell.fillAmount != Health / maxHealth)
        {
            healthbarDamageTell.fillAmount = Mathf.Lerp(healthbarDamageTell.fillAmount, Health / maxHealth, healthBarDamping);
        }

        if (Health <= 0)
        {
            Death();
        }
        if (dialoguestarted)
        {
            if (!myDialogue.isTyping)
            {
                myAnim.SetFloat("animationTimeScale", 1);


            }
        }
        if (takeDamageCounter > 1)
        {


            attackAnyway = true;
            myAnim.SetBool("ForceAttack", attackAnyway);
            rotsputenState = BossState.Attack;
            takeDamageCounter = 0;

        }
    }

    public void resetAttackAnyway()
    {
        attackAnyway = false;
        myAnim.SetBool("ForceAttack", attackAnyway);
    }
    private void FixedUpdate()
    {
        ApplyDeceleration();

        if (ismoving) Move();


      
        velocityClampAndRotationHandlerUpdate();
        if (takeDamageCounter > 1)
        {
            int evadeDir = player.transform.position.x > transform.position.x ? -1 : 1;
            myRigidBody.AddForce(Vector2.right * evadeDir * 10, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 13)
        {
            VcamManager.instance.bossFight = true;
            rotsputenState = BossState.SpellCast;
            GetComponent<BoxCollider2D>().enabled = false;
            myAnim.SetBool("playerInArena", true);
            if(onBossBattleStart!=null)
            {
                onBossBattleStart();
            }
        }
    }









private void SpellCooldownTimer()
    {
        if (startCooldownTimer)
        {
            spellcastCooldown -= Time.deltaTime;
        }
        if (spellcastCooldown <= 0)
        {
            startCooldownTimer = false;
            spellcastCooldown = 50;
        }
    }

    private void velocityClampAndRotationHandlerUpdate()
    {
        myRigidBody.velocity = new Vector2(Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed, maxSpeed),
          Mathf.Clamp(myRigidBody.velocity.y, -maxSpeed * 1.2f, maxSpeed * 1.2f));

        rotationHandler.eulerAngles = mySpriteRenderer.flipX ? new Vector2(0, 180) : new Vector2(0, 0);
    }

    public void BossStateUpdate()
    {
        switch (rotsputenState)
        {
            case BossState.SelectDestination:
                StartCoroutine(PickDestination());

                break;


            case BossState.Move:
                // find navPoint
                // check navPoint
                // set state to navPoint state
                ismoving = true;

                break;
            case BossState.Attack:
                // facePlayer
                CastOrb();
                break;

            case BossState.SpellCast:
                Necromancy();
                break;



            case BossState.Idle:

                myAnim.SetTrigger("Idle");

                break;

            case BossState.Death:
                break;


        }

    }
    IEnumerator PickDestination()
    {
        TargetDestination = NavPts[Random.Range(0, NavPts.Length)].position;
        yield return new WaitForSeconds(.4f);
        rotsputenState = BossState.Move;

    }
    void facePlayer() // face play on attack states
    {
        mySpriteRenderer.flipX = player.transform.position.x < transform.position.x ? true : false;


    }
    private void Move()
    {
        if (rotsputenState == BossState.Move)
        {
            if (!isGrounded) moveSpeed = 2;

            CheckForObstacles();
            float distX = TargetDestination.x - transform.position.x;
            if (Mathf.Abs(distX) > .2f)
            {
                myAnim.SetBool("Run", true);
                myRigidBody.AddForce(Vector2.right * distX * moveSpeed, ForceMode2D.Force);

                mySpriteRenderer.flipX = distX > 0 ? false : true;

            }
            else // reached destination 
            {
                myAnim.SetBool("Run", false);
                if (!startCooldownTimer)
                {
                    ismoving = false;

                    rotsputenState = BossState.SpellCast;
                }
                else if(startCooldownTimer)
                {
                    ismoving = false;

                    rotsputenState = BossState.Attack;
                }
            }
        }


    }// end move()

    private void ApplyDeceleration()
    {
        if (!ismoving) // out of move state may still have velocity lets slow this guy down
        {
            if (Mathf.Abs(myRigidBody.velocity.x) > 0)
            {
                float _decelerationForce = myRigidBody.velocity.x / .01f * myRigidBody.mass; //Deceleration 


            }
            if (myRigidBody.velocity.x == 0)
            {

            }
        }
    }
    public void CastOrb()
    {
        facePlayer();
        myAnim.SetTrigger("Attack");
        // shoot bouncy orb towards player


    }
    void CheckForObstacles()
    {
        raycastHit = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + .5f), ObstacleCheckTransform.position, obstacles);

    }
    public void Necromancy()
    {
        facePlayer();
        myAnim.SetTrigger("Spellcast");
        startCooldownTimer = true;
        Debug.Log("necromancy was called");
        VcamManager.instance.CamShake(3f, 2);

        // apples in 
        // idle up top while player fights apples
        // apears center of arena apples spawn until you destroy it
        // destroying it damages rotsputen aswell

    }

    public override void TakeDamage(int damageValue)
    {
        if (!attackAnyway)
        {
            takeDamageCounter++;

            Health -= damageValue;
            healthBar.fillAmount = Health / maxHealth;


            myAnim.SetTrigger("Hurt");

        }
    }

    public override void Death()
    {
        if(OnBossdeath!=null)
        {

        OnBossdeath();
        }
        myAnim.SetBool("Death", true);
        rotsputenState = BossState.Death;
    }
    public void Jump()
    {
        if (isGrounded)
        {
            myAnim.SetTrigger("Jump");
            myRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
         

        }
    }
    public void GroundedCheck()
    {

        myAnim.SetFloat("velocityY", velocityY = myRigidBody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, whatIsGround);
        wtfAmIHitting = Physics2D.OverlapCircle(transform.position, groundCheckRadius, whatIsGround);

        myAnim.SetBool("Grounded", isGrounded);


    }
    // Animation Events

    public void WildMagic()
    {
        // choose Spell
        currentSpell = Random.Range(0, spells.Count);
        switch (currentSpell)
        {
            case 0:
                enemySpawner.SetActive(true);
                // spawn apples // fight
                break;
            case 1:
                enemySpawner.SetActive(true);
                // spawn rockfall // evade
                break;
            case 2:
                enemySpawner.SetActive(true);
                // spaw the rot wave // jump over it 
                break;

        }

    }
    IEnumerator SpawnAppleTimer()
    {
        spells.RemoveAll(item => item == 0);
        yield return new WaitForSeconds(60);
        spells.Add(0);


    }
    public void SpawnOrb()
    {
        if (rotsputenState != BossState.Death)
        {
            Debug.Log("spawnOrb");
            GameObject OrbODeath = Instantiate(orbOfDeath, handPosition.position, handPosition.rotation);
            rotsputenState = BossState.SelectDestination;

        }




    }
    public void postSpellWait()
    {
        // animation event
       
        StartCoroutine(idleWait());
    }
    IEnumerator idleWait()
    {
        rotsputenState = BossState.PostSpellWait;
        // wait 
        // check which spell, if enemies are left 
        // if they are then shoot balls from up top
        // otherwise jump down and shoot balls 
        yield return new WaitForSeconds(4);
        rotsputenState = BossState.SelectDestination;

    }
    void MyDialogue()
    {
        myAnim.SetFloat("animationTimeScale", 0);
        dialoguestarted = true;
        myDialogue.startTalking();

    }
    public void DisableTalkBubble()
    {
        myDialogue.talkBubble.SetActive(false);

    }
    void QueueEndGameMenu()
    {
        MenuManager.instance.QueueEndGameMenu();
    }
}
