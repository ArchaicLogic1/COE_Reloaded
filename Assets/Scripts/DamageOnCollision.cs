using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] int damageAmt;
    [SerializeField] LayerMask whoToDamage;
   [SerializeField] float gravitateToSpikesForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        Debug.Log("enemy something touched me");
        if(collision.gameObject.layer== 9)
        {
            Debug.Log("takeDamage");
            GameManager.instance.TakeDamage(damageAmt);
        }
        else 
            collision.gameObject.GetComponent<Hackable>().TakeDamage(100);
    }
   
}
