using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollect : MonoBehaviour
{
    Animator myAnim;
   public  float animationTimingOffset;
    SpriteRenderer mySpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
       myAnim.SetFloat("cycleOffset", animationTimingOffset = Random.Range(0.1f, 1.0f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9 || collision.gameObject.layer == 13)// playerlayer;
        {
            AudioManager.instance.playCoinPickup();
            GameManager.instance.AwardPlayerPoints(10, transform.position);
            Destroy(gameObject);

        }
    }
   
}
