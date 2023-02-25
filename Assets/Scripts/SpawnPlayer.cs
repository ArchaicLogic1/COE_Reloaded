using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    GameObject player;
    SpawnPlayer instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
     //  DontDestroyOnLoad(gameObject);
     //  if (instance == null)
     //  {
     //      instance = this;
     //  }
     //  else { Destroy(gameObject); }

        player = GameObject.Find("PlayerController");
        player.transform.position = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
