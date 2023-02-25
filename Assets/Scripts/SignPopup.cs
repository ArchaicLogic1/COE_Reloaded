using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignPopup : MonoBehaviour
{
    [SerializeField] string signDetailLine1, signTitle;
   public  bool AbleToRead;
    [SerializeField] GameObject interactTell;
    Animator myAnimator;
    // Start is called before the first frame update

    private void Start()
    {
        interactTell.SetActive(false);
        myAnimator = GetComponentInChildren<Animator>();
        GameInputManager.interact.performed += ReadSign;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactTell.SetActive(true);
        if(collision.gameObject.layer==9)// player layer
        {
            AbleToRead = true;
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactTell.SetActive(false);
        if (collision.gameObject.layer == 9)// player layer
        {
            AbleToRead = false;
            PopUpManager.instance.ExitPopUp();
          
        }

    }
    private void ReadSign(InputAction.CallbackContext context)
    {
        if(AbleToRead)
        {
            interactTell.SetActive(false);
            PopUpManager.instance.Readsign(signTitle, signDetailLine1);
            AbleToRead = false;
           
        }
       
    }


}

