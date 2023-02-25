using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeRenderer : MonoBehaviour
{
    LineRenderer myLineRenderer;
   [SerializeField] Transform[] bonePositions;
    
    // Start is called before the first frame update
    void Start()
    {
       myLineRenderer = GetComponent<LineRenderer>();
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i = 0; i < myLineRenderer.positionCount; i++)
        {
            myLineRenderer.SetPosition(i, bonePositions[i].position);
        }
        
    }

}
