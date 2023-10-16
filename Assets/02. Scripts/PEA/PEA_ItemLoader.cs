using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PEA_ItemLoader : MonoBehaviour
{
    public enum SearchType
    {
        Image,
        Video,
        Object
    }

    private SearchType searchType = SearchType.Image;

    public GameObject itemSlot;
    public Transform content;
    public bool isMarket = false;

    private void OnEnable()
    {
        AssetDatabase.Refresh();
    }

    void Start()
    {
        LoadItems(searchType);
    }

    void Update()
    {
        
    }

    public void LoadItems(SearchType loadItemType)
    {
        switch (loadItemType)
        {
            case SearchType.Image:
                Texture2D[] itemSprites = Resources.LoadAll<Texture2D>(isMarket ? "Market_Item_Sprites" : "MyItems_Sprites");

                for (int i = 0; i < itemSprites.Length; i++)
                {
                    GameObject slot = Instantiate(itemSlot, content);
                    if(slot.TryGetComponent<PEA_MyItemSlot>(out PEA_MyItemSlot myItemSlot))
                    {
                        myItemSlot.SetItemInfo(new Item(Item.ItemType.Image, itemSprites[i].name, itemSprites[i]));
                        myItemSlot.canvas = transform.parent;
                    }
                    else if(slot.TryGetComponent<PEA_MarketItemSlot>(out PEA_MarketItemSlot marketItemSlot))
                    {
                        marketItemSlot.SetItemInfo(new Item(Item.ItemType.Image, itemSprites[i].name, itemSprites[i]));
                    }
                    slot.GetComponentInChildren<RawImage>().texture = itemSprites[i];
                }
                break;
            case SearchType.Video:
                break;
            case SearchType.Object:
                break;
        }
    }
}
