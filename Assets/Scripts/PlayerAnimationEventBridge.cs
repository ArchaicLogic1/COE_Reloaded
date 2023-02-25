using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationEventBridge : MonoBehaviour
{
    [SerializeField]Animator myanim;
  
    Rigidbody2D myRigidBody;

    [Header("Attack")]
    [SerializeField] Transform hitBoxTransform;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask Hitable;
    public Collider2D[] hitColliders;

    public delegate void AttackEnd();
    public static event AttackEnd OnAttackEnded;

    [Header("Audio")]
    [SerializeField] List<AudioClip> SoundFX = new List<AudioClip>();  
    AudioSource myAudioSource;
    public static  List<AudioClip> soundFXRef;
    SpriteRenderer mySpriteRenderer;
    int directionModifier;
    [SerializeField] int damageValue = 10;


    // Start is called before the first frame update
    void Awake()
    {
        InitializeMembers();
    }

   

    public void Start()
    {
        GameInputManager.jump.performed += PlayJumpSFX;
        GameManager.onDamageTaken += DamageReaction;
    }

    // Update is called once per frame
    void Update()
    {
        directionModifier = PlayerController.instance.moveDir;

    }
    private void InitializeMembers()
    {
        soundFXRef = SoundFX;

        myAudioSource = GetComponent<AudioSource>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponentInParent<Rigidbody2D>();
    }
    public void ResetAttackCounter()
    {
        myanim.SetInteger("AttackCounter", 0);

        if (OnAttackEnded != null)
        {
            OnAttackEnded();
        }

    }
    public void PlaySoundFX(AnimationEvent AnimEvent)
    {

        int index = AnimEvent.intParameter;
        float volumeControl = AnimEvent.floatParameter;
        string DoWeRandomize_YorN = AnimEvent.stringParameter;

        if (SoundFX.Count - 1 >= index)
        {
            if (DoWeRandomize_YorN == "Y") { myAudioSource.pitch = Random.Range(.7f, 1); }
            else
            {
                myAudioSource.pitch = 1;

            }
            myAudioSource.volume = volumeControl;
            myAudioSource.PlayOneShot(SoundFX[index]);

        }
    }
    public void PlayFootStepSoundFX(AnimationEvent AnimEvent)
    {

        int index = Random.Range(1, 3);
        float volumeControl = AnimEvent.floatParameter;

        string DoWeRandomize_YorN = AnimEvent.stringParameter;

        if (SoundFX.Count - 1 >= index)
        {
            myAudioSource.volume = volumeControl;
            myAudioSource.pitch = Random.Range(.7f, 1.2f);

            myAudioSource.PlayOneShot(SoundFX[index]);

        }
    }
    public void WallSlideSoundFX()
    {

        myAudioSource.clip = SoundFX[6];
        myAudioSource.Play();

    }

    public void Impulse(int forceAmount = 0)
    {
        myRigidBody.AddForce(Vector2.right * directionModifier * forceAmount, ForceMode2D.Impulse);

    }
    public void HitBox()
    {
        hitColliders = Physics2D.OverlapCircleAll(hitBoxTransform.position, attackRadius, Hitable);

        if (hitColliders.Length > 0)
        {
            VcamManager.instance.CamShake(1.5f, .1f);
            AudioManager.instance.PlayHitSFX();
            Debug.Log("hit");
            if (!myanim.GetBool("Grounded"))
            {
                myRigidBody.AddForce(Vector2.up * 8, ForceMode2D.Impulse);

            }

            for (int i = 0; i < hitColliders.Length; i++)
            {

                hitColliders[i].GetComponent<Hackable>().TakeDamage(damageValue);

            }
        }
    }
    public void PlayJumpSFX(InputAction.CallbackContext context)
    {
        myAudioSource.volume = .5f;
        myAudioSource.PlayOneShot(SoundFX[5]);

    }
    public void AlterLinearDrag(float drag)
    {
        myanim.SetFloat("Drag", drag);
    }
    public void DamageReaction()
    {
        Debug.Log("damageReaction called");
        myanim.SetTrigger("TakeDamage");
       
    }
    public void CamShake()
    {
        VcamManager.instance.CamShake(3f, .1f);
    }
}
