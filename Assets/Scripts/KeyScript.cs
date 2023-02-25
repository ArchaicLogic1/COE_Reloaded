using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            AudioManager.instance.playCoinPickup();
            GetComponentInParent<OpenChest>().keyFound = true;
            gameObject.SetActive(false);
        }
    }
}
