using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Dialogue : TextTyper
{
 
   [SerializeField] GameObject interactTell;
    public bool canTalk;
    Animator talkBubbleAnim;
   public GameObject talkBubble;
 
  [SerializeField]Canvas myCanvas;
    
  
    // Start is called before the first frame update
    void Start()
    {
        talkBubble.SetActive(false);
        myCanvas.worldCamera = Camera.main;
        Debug.Log(typingSpeed);

    }

    // Update is called once per frame
    void Update()
    {

       


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)// player layer
        interactTell.SetActive(true);
        
        GameInputManager.interact.performed += startTalking;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)// player layer
            interactTell.SetActive(false);
        canTalk = false;
        GameInputManager.interact.performed -= startTalking;
        talkBubble.SetActive(canTalk ? true : false);
    }
    void startTalking(InputAction.CallbackContext   context)
    {
        GameInputManager.interact.performed -= startTalking;
        canTalk = true;
        talkBubble.SetActive(canTalk ? true : false);
       
        Debug.Log("startTalk");
        if (canTalk == true)
        { StartCoroutine(TextTyper3000(sentences, 0, TextToType, .8f));
            Debug.Log("shouldTalk");
       
        canTalk = false;
        }
        
    }
    public void startTalking()
    {
       
        canTalk = true;
        talkBubble.SetActive(canTalk ? true : false);

        Debug.Log("startTalk");
        if (canTalk == true)
        {
            StartCoroutine(TextTyper3000(sentences, 0, TextToType, .8f));
            Debug.Log("shouldTalk");

            canTalk = false;
        }
    }
    void playMyDialogue()
    {
 //  if(isTyping)
 //  {
 //      
 //      if(!myAudio.isPlaying)
 //      {
 //          myAudio.pitch = Random.Range(.85f, .92f);
 //          myAudio.clip = myVoice[Random.Range(0, myVoice.Count)];
 //          myAudio.Play();
 //      }
 //  }
    }

}
