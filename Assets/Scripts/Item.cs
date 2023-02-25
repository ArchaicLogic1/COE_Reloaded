using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]  enum TypeOfItem { Invincibility,PointBoost,key,rareItem,map}
    [SerializeField]public  Sprite mySourceImage;
    [SerializeField]public  int pointValue;
    [SerializeField] TypeOfItem typeOfItem;
    public string itemDescription;
    public bool awardsPoints, hasItemTimer;
    public int itemIndex;
            


    private void OnEnable()
    {
        // tell the game manager how many points 
        // show image in UI
        // store item in collection
        // at the end it shows if you found all the possible items. 
        // at the begining of the game all chest and items roll spawn 
        // all items show at the end 12/ 25 items for example
        // all item bonus is huge the completionist
        
    }


}
