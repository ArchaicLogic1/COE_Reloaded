using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenChest : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] bool keyRequired; // for me to tick upon placement of prefab

    public bool keyFound;
    public string popUpMessage;
    [SerializeField] GameObject myitemContainer;
    [SerializeField] public Item myItem;
    bool chestOpened;
    [SerializeField] GameObject interactTell;
    [SerializeField]interactIcon interactIconSwapScript;
    BoxCollider2D myTrigger;
    [SerializeField] SpriteRenderer chestSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myItem = myitemContainer.GetComponent<Item>();
        myTrigger = GetComponent<BoxCollider2D>();
        interactTell.SetActive(false);
       // myItem = GlobalItemStorage.intance.ItemtoWorldDelivery();
        myAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)// 9 is player layer
        {
            interactTell.SetActive(true);


            if (keyRequired && keyFound && !chestOpened)
            {
                Debug.Log("infront of chest");
                // interact popup

                interactIconSwapScript.setInteractIcon();

                GameInputManager.interact.performed += OpenDaChest;

            }
            else if (keyRequired && !keyFound)
            {
                interactIconSwapScript.setLockedIcon();
                // dispay key is needed 
                // lockedIcon
            }
            else if(!keyRequired)
            {
                GameInputManager.interact.performed += OpenDaChest;
                interactIconSwapScript.setInteractIcon();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
      //  GameInputManager.interact.performed -= OpenDaChest;
        interactTell.SetActive(false);
    }
    public void OpenDaChest(InputAction.CallbackContext context)
    {
        Debug.Log("openDaChest");
        chestOpened = true;
        GameInputManager.interact.performed -= OpenDaChest;
        myAnimator.SetBool("Open", true);
        PopUpManager.instance.FoundItem(myItem);
        myTrigger.enabled = false;
        chestSpriteRenderer.color = Color.gray;
        // show item in UI 
        // pause game
        // play popup 
        // continue
        // propigate score
    }
    


}
