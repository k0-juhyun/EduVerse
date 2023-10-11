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
                    slot.GetComponentInChildren<RawImage>().texture = itemSprites[i].texture;
                }
                break;
            case MarketType.Video:
                break;
            case MarketType.Object:
                break;
        }
    }
}
