using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PopUpManager : MonoBehaviour
{
   public static PopUpManager instance;
    public GameObject popUp;
    [SerializeField]  Image icon, shadow;
    [SerializeField] TMPro.TMP_Text popUpBody, popUpTitle, timerText;
   [SerializeField] Button okButton;
    AudioSource myAudioSource;
    int pointValue;
    bool awardPoints = false;
    bool itemTimerCountDown = false;
    int itemTimer;
   [SerializeField] GameObject player;
   [SerializeField] bool fadeIn, fadeOut;
    [SerializeField] CanvasGroup mycanvasGroup;
    [SerializeField] float fadeSpeedMovifier=1;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
   // if (instance == null)
   // {
   //     instance = this;
   //
   // }
   // else
   // {
   //     Destroy(gameObject);
   // }
        myAudioSource = GetComponent<AudioSource>();
        okButton.onClick.AddListener(ExitPopUp);
       
       // GameInputManager.interact.performed += ExitPopUpWithInteract;
        
    }

    // Update is called once per frame
    void Update()
    {
        FadeController();


    }
    public void FoundItem(Item item, bool delay = false)
    {
        GameInputManager.interact.performed += ExitPopUpWithInteract;
        itemTimerCountDown = item.hasItemTimer;

        CollectUIScript.instance.ItemUISetActive(item.itemIndex);


        // wait for pop up  ex. allow chest to start opening
        if (delay)
        {
            StartCoroutine(OpenPopup(item));

        }
        else
        {
            SetPopUpDependencies(item);
        }


       
//   StartCoroutine(ExitPopUpOnTimer());



    }

    private void SetPopUpDependencies(Item item)
    {
        instance.popUpTitle.text = null;
        instance.icon.enabled = true;
        instance.shadow.enabled = true;
        awardPoints = item.awardsPoints;
        pointValue = item.pointValue;
        myAudioSource.Play();
        PopUpFadeIn();
        instance.icon.sprite = item.mySourceImage;
        instance.shadow.sprite = item.mySourceImage;
        instance.popUpBody.text = item.itemDescription;
    }

    public void Readsign(string _title, string _body, Sprite icon=null)
    {
        popUpTitle.text = _title;
        popUpBody.text = _body;
        PopUpFadeIn();
        if (icon == null)
        {
        instance.icon.enabled = false;
        instance.shadow.enabled =false;
        }
        else
        {
        instance.icon.sprite = icon;
        instance.shadow.sprite = icon;

        }
        // StartCoroutine(ExitPopUpOnTimer());
        awardPoints = false;
        GameInputManager.interact.performed += ExitPopUpWithInteract;

    }
    public void ExitPopUp()
    {
        PopUpFadeOut();
        if (awardPoints)
        {
            GameManager.instance.AwardPlayerPoints(pointValue, player.transform.position);
            awardPoints = false;

        }
     //   GameInputManager.interact.performed -= ExitPopUpWithInteract;

    }
    public void ExitPopUpWithInteract(InputAction.CallbackContext context)
    {
        ExitPopUp();
    }
   
   IEnumerator ExitPopUpOnTimer()
    {

        yield return new WaitForSeconds(5);
        ExitPopUp();
    }
 
        
    
   IEnumerator OpenPopup(Item item)
    {
        yield return new WaitForSeconds(.5f);
        SetPopUpDependencies(item);
    }
   void PopUpFadeIn()
    {
        fadeIn = true;
    }
   void PopUpFadeOut()
    {
        fadeOut = true;
       
    }
    public void FadeController()
    {
        if(fadeIn)
        {
            fadeOut = false;
            popUp.SetActive(true);
            if(mycanvasGroup.alpha<=1)
            {
                mycanvasGroup.alpha += Time.deltaTime * fadeSpeedMovifier;
                if (mycanvasGroup.alpha >= 1)
                {

                    fadeIn = false;

                }

            }
        }
        if(fadeOut)
        {
            fadeIn = false;
            if(mycanvasGroup.alpha>=0)
            {
                mycanvasGroup.alpha -= Time.deltaTime * fadeSpeedMovifier;
                if(mycanvasGroup.alpha==0)
                {
                    popUp.SetActive(false);
                    fadeOut = false;

                }
            }    
        }



    }

}
