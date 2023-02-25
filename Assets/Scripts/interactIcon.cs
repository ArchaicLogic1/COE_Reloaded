using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactIcon : MonoBehaviour
{
    [SerializeField]SpriteRenderer myLockedIcon;
   [SerializeField] SpriteRenderer myInteractIcon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setLockedIcon()
    {
        myLockedIcon.enabled = true;
        myInteractIcon.enabled =false;

    }
    public void setInteractIcon()
    {
        myLockedIcon.enabled = false;
        myInteractIcon.enabled = true;

    }
}
