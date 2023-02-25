using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalItemStorage : MonoBehaviour
{
  public  List<Item> GlobalItemList = new List<Item>();
    List<Item> givenAwayItems = new List<Item>();
   public static GlobalItemStorage intance;
    // Start is called before the first frame update
  
    void Awake()
    {
        intance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item ItemtoWorldDelivery()
    {
        int _itemcounter = 0;
         Item yourNewItem = GlobalItemList[Random.Range(0, GlobalItemList.Count)];
        foreach(Item _item in givenAwayItems)
        {
            if(_item== yourNewItem)
            {
                _itemcounter++;

            }
            if(_itemcounter>1)
            {
                GlobalItemList.Remove(_item);
            }
        }
        givenAwayItems.Add(yourNewItem);

        return yourNewItem;
    }
}
