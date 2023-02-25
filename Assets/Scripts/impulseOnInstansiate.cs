using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impulseOnInstansiate : MonoBehaviour
{
    int dir;
    public float orbForce;
    Rigidbody2D myRigidbody;
    int clampRangeval;
    public ParticleSystem deathParticle;
    // Start is called before the first frame update
    void Start()
    {
        clampRangeval = Random.Range(0, 15);
        dir = transform.rotation.y == 0 ? 1 : -1;
        myRigidbody = GetComponent<Rigidbody2D>();
            myRigidbody.AddForce(Vector2.right * dir *orbForce, ForceMode2D.Impulse);
       
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(Mathf.Clamp(myRigidbody.velocity.x, -8, 8), Mathf.Clamp(myRigidbody.velocity.y, -clampRangeval,clampRangeval));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer==9)
        { 
              deathParticle.Play();
            deathParticle.transform.parent = null;
            Destroy(deathParticle,2);
            
          GameManager.instance.TakeDamage(Random.Range(200,350));
          Destroy(gameObject);
           
           
            
        }
    }
    public void DestroyOnBossDeath()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

}
