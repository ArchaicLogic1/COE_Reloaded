using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public float offset;
    public float maxXdist;
    public float minXDist;
   public  float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x-objectToFollow.position.x)> minXDist)
        {
            transform.position = Vector2.MoveTowards(transform.position, objectToFollow.position, speed * Time.deltaTime);
        }
       
      
    }
}
