using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_Market : MonoBehaviour
{
    public enum MarketType
    {
        Image,
        Video,
        Object
    }

    private MarketType itemType = MarketType.Image;

    public GameObject itemSlot;
    public Transform content;
    public GameObject imageItem;

    void Start()
    {
        LoadItems(itemType);
    }

    void Update()
    {
        
    }

    public void LoadItems(MarketType loadItemType)
    {
        switch (loadItemType)
        {
            case MarketType.Image:
                Sprite[] itemSprites = Resources.LoadAll<Sprite>("Market_Item_Sprites");

                for (int i = 0; i < itemSprites.Length; i++)
                {
                    GameObject slot = Instantiate(itemSlot, content);
                    slot.GetComponent<PEA_ItemSlot>().item = new Item(Item.ItemType.Image, itemSprites[i].name, itemSprites[i]);
                    slot.GetComponent<PEA_ItemSlot>().market = this;
                    slot.GetComponent<PEA_ItemSlot>().canvas = transform.parent;
                    slot.GetComponentInChildren<RawImage>().texture = itemSprites[i].texture;
                }
                break;
            case MarketType.Video:
                break;
            case MarketType.Object:
                break;
        }
    }

    public GameObject GetItem(Item item)
    {
        GameObject getItem = Instantiate(imageItem);
        getItem.GetComponent<Image>().sprite = item.itemSprite;
        getItem.GetComponent<Image>().preserveAspect = true;


        return getItem;
    }
}
