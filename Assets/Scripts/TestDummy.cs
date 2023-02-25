using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : Hackable
{
    Animator myAnim;
    int hitCounter = 0;
   [SerializeField] ParticleSystem mydeathParticle;
    void Start()
    {
        myAnim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if(hitCounter>=5)
        {
            Death();
        }
    }
    // Update is called once per frame

    public override void Death()
    {
        mydeathParticle.Play();
        Destroy(mydeathParticle.gameObject, 3);
        mydeathParticle.gameObject.transform.parent = null;
        Destroy(gameObject);
     

    }

    public override void TakeDamage(int damageValue)
    {
        
        myAnim.SetTrigger("hit");
        hitCounter++;
    }

    // Start is called before the first frame update
}
