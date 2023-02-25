using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //physics member variables
  
    [Header("Movement")]
    [SerializeField] float moveForce;
    [SerializeField] float maxMoveSpeed;
    private Rigidbody2D myRigidBody;
    private Vector2 moveInputDirection;

    [SerializeField] float jumpForce,WallJumpForce;
    [SerializeField] float bonusGravity;
    float yVelocity;
  
   

    private bool wallJumpInputLock = false;

    public static bool isGrounded,isRunning;
    private bool wallStop;

    [Header("Collision Sensors")]
    [SerializeField] Transform groundCheckTransform, rotationParentTransform;
    [SerializeField] float  groundCheckRadius;
    public LayerMask whatIsGround, whatIsWall;
   


    Animator myAnim;
    SpriteRenderer mySpriteRenderer;
    private int attackCounter = 0;
    public  int moveDir=1;
    public float xVel;

    // defensive bools
    bool isSliding;
    bool isAttacking;
    bool isAirAttacking;

    // wallSliding members
    [Header("Wallslide")]
    [SerializeField] Transform wallCheckTransform;
    [SerializeField] float wallSlideDragValue,wallSlideDecayModifier;

    AudioSource myAudioSource;
    float airTimer;
    private int jumpCounter =0, jumpLimit=1;
    private bool canMove;
  
   
    [Header("Deceleration")]
    [SerializeField]  float decelerationTime, jumpDecelerationTime;
    [SerializeField] float deceleration;
  
    [Header("Velocity Limits")]
    [SerializeField] float terminalVelocity;

    bool variableGravityActive,decelerationActive;
    public static PlayerController instance;
    private void Awake()
    {
     instance = this;
    // DontDestroyOnLoad(gameObject);
    //    if (instance == null) { instance = this; }
    //    else { Destroy(gameObject); }

        InitializeMembers();

        PlayerAnimationEventBridge.OnAttackEnded += EndAttack;
    }

   

    void Start()
    {
       
        SubscribeInputSystem();
    }

    // Update is called once per frame
    void Update()
    {


        ReadHorizontalInput();

        VelocityCapture();
        GroundCheck();

        WallCheck();
       
        myRigidBody.gravityScale = yVelocity < 0 ? 3.5f : 2.5f;    // JumpFeel gravity modification

        CoyoteAirTimer();

        MovementDirectionAndRotationHandler();

    }

   

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        ApplyVariableGravity();
        LinearDragHandler();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("SlideTunnel"))
        {
            myRigidBody.AddForce(Vector2.right * moveDir * 8, ForceMode2D.Impulse); // pushes player through slide prevents geting stuck in tunnel
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }




    private void OnDisable()
    {
        UnSubscribeInputSystem();
    }



    private void InitializeMembers()
    {
        myAudioSource = GetComponentInChildren<AudioSource>();
        myAnim = GetComponentInChildren<Animator>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }



    /// <summary>
    /// reads player input wasd/ gamepad left stick and stores to moveIputDirection
    /// </summary>
    private void ReadHorizontalInput()
    {
        moveInputDirection = GameInputManager.move.ReadValue<Vector2>();
        canMove = Mathf.Abs(moveInputDirection.x) > .3f ? true : false;
    }
    public void HorizontalMovement(InputAction.CallbackContext context)
    {
        decelerationActive = false;
        Debug.Log(moveDir);
        //  FlipRotationParent();


    }

    /// <summary>
    /// Based on Horizontal input handles sprite flip and rotational housing Y rotation for dependent children ex( wall check transform, hitbox transform...)
    /// </summary>
    private void MovementDirectionAndRotationHandler()
    {
        if (GameManager.gameState == GameManager.GameState.InGame)
        {

            if (moveInputDirection.x > 0.3f)
            {
                moveDir = 1;
                mySpriteRenderer.flipX = false;
                rotationParentTransform.eulerAngles = new Vector2(0, 0);
            }
            else if (moveInputDirection.x < -0.3f)
            {
                moveDir = -1;
                mySpriteRenderer.flipX = true;
                rotationParentTransform.eulerAngles = new Vector2(0, 180);
            }
        }
    }

    /// <summary>
    ///  Timer thats activated when not on the ground
    /// </summary>
    private void CoyoteAirTimer()
    {
        if (!isGrounded && airTimer == 0)
        {
            airTimer += Time.deltaTime;
        }
        else if (isGrounded)
        {
            airTimer = 0;
        }
    }





    /// <summary>
    /// captures and stores x and y velocity; also sets my anim Yvelocity for jump/fall
    /// </summary>
    private void VelocityCapture()
    {
        myAnim.SetFloat("Y_Velocity", myRigidBody.velocity.y);
        yVelocity = myRigidBody.velocity.y;
        xVel = myRigidBody.velocity.x;
    }

   

    private void ApplyDeceleration()
    {
        if (decelerationActive)
        {
            if (Mathf.Abs(myRigidBody.velocity.x) > 0)
            {
                float _decelerationForce = myRigidBody.velocity.x / decelerationTime * myRigidBody.mass; //Deceleration 
                myRigidBody.AddForce(Vector2.right * _decelerationForce, ForceMode2D.Force);

            }
            if (myRigidBody.velocity.x == 0)
            {
                decelerationActive = false;
            }
        }
    }

    /// <summary>
    ///  makes variable jump possible, on jump button release applies extra gravity until apex of jump is reached
    /// </summary>
    private void ApplyVariableGravity()
    {
        if (variableGravityActive) 
        {
            if (myRigidBody.velocity.y > 0)
            {
                bonusGravity = myRigidBody.velocity.y / jumpDecelerationTime * myRigidBody.mass; //Deceleration 
                myRigidBody.AddForce(Vector2.down * bonusGravity, ForceMode2D.Force);

            }
            if (myRigidBody.velocity.y < 0)
            {
                variableGravityActive = false;
            }
        }
    }
    void variableJump(InputAction.CallbackContext context)
    {

        Debug.Log("jump Released babayy");
        variableGravityActive = true;

    }


    void Jump(InputAction.CallbackContext context)
    {
        // airTimer allows for coyote timing ... gives grace to the player for better game feel
        if (!myAnim.GetBool("WallSlide") && isGrounded && (jumpCounter < jumpLimit || airTimer < .1f && jumpCounter < jumpLimit)) 
        {
            jumpCounter++; 
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
            myRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        }

    }

   
   

    private void SubscribeInputSystem()
    {

      
       GameInputManager.jump.performed += Jump;
       GameInputManager.jump.canceled += variableJump;
       GameInputManager.slide.performed += Slide;
       GameInputManager.move.performed += HorizontalMovement;
       GameInputManager.melee.performed += Attack;

       myRigidBody = GetComponent<Rigidbody2D>();

    }
    private void UnSubscribeInputSystem()
    {


        GameInputManager.jump.performed -= Jump;
        GameInputManager.jump.canceled -= variableJump;
        GameInputManager.slide.performed -= Slide;
        GameInputManager.move.performed -= HorizontalMovement;
        GameInputManager.melee.performed -= Attack;


    }
   

    /// <summary>
    ///  handles velocity clamping, aswell as movement speed based on horizontal input 
    /// </summary>
    private void SetPlayerVelocity()
    {
       

        if (!wallJumpInputLock)
        {
            myRigidBody.velocity = new Vector2(Mathf.Clamp(myRigidBody.velocity.x, -maxMoveSpeed, maxMoveSpeed), 
            (Mathf.Clamp(myRigidBody.velocity.y, -terminalVelocity, terminalVelocity))); 
        


             if (isGrounded && !isAttacking && !myAnim.GetBool("WallSlide") && !wallStop && canMove)
             {
                myAnim.SetBool("Run", true);
                isRunning = true;
                myRigidBody.AddForce(Vector2.right * moveInputDirection * moveForce, ForceMode2D.Force);

             }
            else if(!isGrounded && !myAnim.GetBool("WallSlide"))
            {
                // airMovement 1/2 ground movespeed
  
             myRigidBody.AddForce(Vector2.right * moveInputDirection * moveForce*.5f, ForceMode2D.Force);

            }
            
            else
            {
                myAnim.SetBool("Run", false);
                isRunning = false;
            }

        }

    }

    /// <summary>
    /// deceleration, and wallslide friction applied through the rigidbody Linear drag
    /// </summary>
    public void  LinearDragHandler()
    {
        if(Mathf.Abs( myRigidBody.velocity.x)>.01f && isGrounded && Mathf.Abs(moveInputDirection.x)<.01f && !isSliding)
        {
            myRigidBody.drag = deceleration;
        }
        else if( myAnim.GetBool("WallSlide"))
        {
            myRigidBody.drag = wallSlideDragValue;
           // add drag decrement (polish)
           
        }
        else if (myAnim.GetFloat("Drag")>0)
        {
            myRigidBody.drag = myAnim.GetFloat("Drag");
        }
        else
        {
            myRigidBody.drag = 0;
        }
    }

           

           
          
   
    



  


    void Attack(InputAction.CallbackContext context)
    {
        if(GameManager.gameState==GameManager.GameState.InGame)
        { 
        Debug.Log("attack ");
        if(isGrounded)
        {
        isAttacking = true;
        myAnim.SetTrigger("Attack");
        attackCounter = myAnim.GetInteger("AttackCounter");
        attackCounter += 1;
        myAnim.SetInteger("AttackCounter",attackCounter);

        }
        else if ( !isGrounded)
        {
            Debug.Log("air attack");
            isAirAttacking = true;
            myAnim.SetTrigger("AirAttack");
            attackCounter += 1;
            myAnim.SetInteger("AttackCounter", attackCounter);
        }
        if(isAirAttacking && isGrounded)
        {
            isAirAttacking = false;
            myAnim.ResetTrigger("AirAttack");
        }

        }
    }

   
   

    /// <summary>
    /// physics check for ground sets animator bool Grounded
    /// </summary>
    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle (groundCheckTransform.position, groundCheckRadius,whatIsGround);
        myAnim.SetBool("Grounded", isGrounded);
        if(isGrounded)
        {
            jumpCounter = 0;
        }
      
      
    }
    
   
 
    /// <summary>
    /// flips housing tranform for physics checks related to the players facing direction ex( linecast transform references)
    /// </summary>
    private void FlipRotationParent()
    {
       
        rotationParentTransform.eulerAngles = mySpriteRenderer.flipX ? new Vector2(0, 180) : new Vector2(0, 0);
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if(!isSliding && Mathf.Abs(moveInputDirection.x)>.2f )
        {
        
        myAnim.SetTrigger("Slide");
        isSliding = true;
        Debug.Log("slide");
            StartCoroutine("SlideTimer");
        }
    }

    IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(.3f);
        isSliding = false;
    }
   
    public void EndAttack()
    {
        isAttacking = false;
    }


    /// <summary>
    /// lines check for wallslide and obstacle check
    /// </summary>
    public void WallCheck()
    {
        RaycastHit2D _rayHit = Physics2D.Linecast(transform.position, wallCheckTransform.position, whatIsWall);

        if (_rayHit && !isGrounded && myRigidBody.drag == 0)
        {
           // myRigidBody.drag = wallSlideDragValue;
            myAnim.SetBool("WallSlide", true);
           GameInputManager.jump.performed += WallJump;
            WallSlideSoundFX();
        }
      
       
        if( !_rayHit || isGrounded & xVel==0)
        {
         // myRigidBody.drag = 0;
            myAnim.SetBool("WallSlide", false);
            GameInputManager.jump.performed -= WallJump;
            WallSlideSoundFX(true);
        }

        
        if (myRigidBody.drag>5)
        {
          //  myRigidBody.drag -= Time.deltaTime * wallSlideDecayModifier;
        }
        
        
        if (_rayHit && isGrounded)
        {
            wallStop = true;
        }
        else 
        {
            wallStop = false;
        }
    }
    /// <summary>
    /// Jump thats subscribed to jump inputAction if player is wallsliding 
    /// </summary>
    /// <param name="context"></param>
    public void WallJump(InputAction.CallbackContext context)
    {       
        WallSlideSoundFX(true);
        mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
        FlipRotationParent();

        myRigidBody.velocity = Vector2.zero;
      
        moveDir = mySpriteRenderer.flipX ? -1 : 1;

        StartCoroutine(wallJumpIgnoreSticks());
        myRigidBody.AddForce(Vector2.up * WallJumpForce, ForceMode2D.Impulse);
        myRigidBody.AddForce(Vector2.right * moveDir * WallJumpForce*.5f, ForceMode2D.Impulse);
        myAnim.SetBool("WallSlide", false);
        




    }
    /// <summary>
    /// timer that ignores horizontal input to prevent player from breaking wall jump eject force by pushing direction of previous wall 
    /// </summary>
    /// <returns></returns>
    IEnumerator wallJumpIgnoreSticks() 
    {
        wallJumpInputLock = true;
        yield return new WaitForSeconds(.3f);
        wallJumpInputLock = false;
    }
   
    public void WallSlideSoundFX(bool stop = false)
    {
        if (stop && myAudioSource.clip== PlayerAnimationEventBridge.soundFXRef[6])
        {
            myAudioSource.clip = null;
        }
        
        else if(myAnim.GetBool("WallSlide"))
        {

            myAudioSource.volume = .05f;
            myAudioSource.pitch = .7f;
        myAudioSource.clip = PlayerAnimationEventBridge.soundFXRef[6];
        myAudioSource.Play();

        }
      

    }
  



}

