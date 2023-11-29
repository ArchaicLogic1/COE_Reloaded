using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TESTsCRIPT : MonoBehaviour
{
    [SerializeField] Button myButton;
    [SerializeField] GameObject myGameObject;
    // Start is called before the first frame update
   delegate void  DisableGameObject();
   event DisableGameObject disableGameObject;
    void Start()
    {
        disableGameObject += SwapActivity;
        disableGameObject += DisableButton;

        myButton.onClick.AddListener(DisableMyGameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   void DisableMyGameObject()
    {
            disableGameObject();
    }
    void SwapActivity()
    {
        myGameObject.SetActive(!myGameObject.activeSelf);

    }
    void DisableButton()
    {
       myButton.enabled = false;
       Invoke("EnableButton", 4);
    }
    void EnableButton()

    {
        myButton.enabled = true;
    }
}
