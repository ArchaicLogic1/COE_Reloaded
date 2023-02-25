using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortiningOrderChanger : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9) // player layer
        {
            if(collision.gameObject.transform.position.x> transform.position.x)
            {
                mySpriteRenderer.sortingOrder = -500;
            }
            else
            {
                mySpriteRenderer.sortingOrder = 500;
            }
        }
    }
}
