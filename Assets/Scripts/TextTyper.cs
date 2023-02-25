using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TextTyper : MonoBehaviour
{
    
    public TMP_Text TextToType;
    public float typingSpeed= .2f;
    public float sentencePause = 1;
    public float commaPause = .5f;
    public AudioSource myAudio;
    public AudioClip mySound;
    public float startDelay=1;
    public bool isTyping;
    public float animationTimeScale;
    // test values
   [SerializeField]
   [TextArea]
    public string[] sentences;

    // Start is called before the first frame update
    void Start()
    {
       // GameInputManager.interact.performed += SkipTyping;
        StartCoroutine(TextTyper3000(sentences, 0, TextToType,startDelay));
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
  public IEnumerator TextTyper3000(string[] sentences, int currentLine, TMP_Text textToTypeIn, float startDelay =0, bool hasOwnAudio=false)
    {
        if(!isTyping)
        {

           
        float currentTypingSpeed= typingSpeed;
       

        textToTypeIn.text = null;
        isTyping = true;

        if (startDelay>0)
        {
        yield return new WaitForSecondsRealtime(startDelay);

        }
        foreach (char Character in sentences[currentLine].ToCharArray())
        {
          
                textToTypeIn.text += Character;
            if(!hasOwnAudio && Character !=' ')
            {
                myAudio.pitch=(Random.Range(.45f, .5f));
                myAudio.PlayOneShot(mySound);

            }
            if(Character==' ')
            {
                currentTypingSpeed = .2f;
            }
           ;
            if (Character == '.')
            {
                
                currentTypingSpeed = sentencePause;
            }
            if (Character == ',')
            { 
               
                    currentTypingSpeed = commaPause;
            }
           else
            {
               
                currentTypingSpeed = typingSpeed;
            }
          
         
           
         
            yield return new WaitForSecondsRealtime(currentTypingSpeed);
        
        }
        isTyping = false;
            
        }

        
    }
    void SkipTyping(InputAction.CallbackContext context)
    {
        StopCoroutine("TextTyper3000");
        TextToType.text = sentences[0];
        
    }
    


}
