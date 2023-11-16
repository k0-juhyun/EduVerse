using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Item : MonoBehaviour
{
    private Item item;

    public Item Item
    {
        get { return item; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetItem(Item item)
    {
        this.item = item;
        switch (item.itemType)
        {
            case Item.ItemType.Image:
                break;
            case Item.ItemType.GIF:
                break;
            case Item.ItemType.Video:
                break;
            case Item.ItemType.Object:
                break;
            default:
                break;
        }
    }
}
