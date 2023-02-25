using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectUIScript : MonoBehaviour
{
    public static CollectUIScript instance;
    public List<GameObject> foundItems = new List<GameObject>();

    private void Awake()
    {
        instance = this;
//    if (instance == null)
//    {
//        instance = this;
//
//    }
//    else
//    {
//        Destroy(gameObject);
//    }
    }
    public void ItemUISetActive(int itemIndex)
    {
        foundItems[itemIndex].SetActive(true);
    }
}
