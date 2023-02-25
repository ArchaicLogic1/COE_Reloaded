using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleAnimationEventBridge : MonoBehaviour
{
    Animator myAnim;
    [SerializeField] Transform hitBoxTransform;
    [SerializeField] float attackRadius;
    public LayerMask  Hitable;
    Apple apple;
  [SerializeField] float  knockBackForce;
    // Start is called before the first frame update
    void Start()
    {
        apple = GetComponentInParent<Apple>();
        myAnim = GetComponent<Animator>();
        if(hitBoxTransform==null)
        {
        
            hitBoxTransform = transform;

        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitBoxTransform.position, attackRadius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EndAttack()
    {
        myAnim.SetBool("Attack", false);
    }
    public void HitBox()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(hitBoxTransform.position, attackRadius, Hitable);
        if(hitCollider!= null)
        {
            int _dirAway = hitCollider.gameObject.transform.position.x > transform.position.x ? -1 : 1;
            hitCollider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -_dirAway * knockBackForce, ForceMode2D.Impulse);
            GameManager.instance.TakeDamage(100);
        }
    }
    public void LungeForward()
    {
      //  apple.LungeForward();
    }
    public void SetPostAttackState()
    {
        apple.SetPostAttackState();
    }

}

