using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Hackable
{
    public LayerMask goodGuys;
    public float moveSpeed;
    public Collider2D victim;
    public  Rigidbody2D myRigidbody;
    public override void Death()
    {
        
    }
    public virtual void OnDrawGizmos(float sightRadius)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    public override void TakeDamage(int damageValue)
    {
    }

    public abstract void Wander(Transform destination);
   

    public virtual bool LookForPlayer(float sightRadius)
    {
      victim = Physics2D.OverlapCircle(transform.position, sightRadius, goodGuys);
        bool _playerFound = Physics2D.OverlapCircle(transform.position, sightRadius, goodGuys);
      if(_playerFound)
        {
       
            // add attack logic

        }

        return _playerFound;
    }
    public virtual void initializer()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
      
    }
    public abstract void SearchForLostVictim();
   
}
