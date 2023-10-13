using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public enum ItemType
    {
        Image,
        Video,
        Object
    }

    public ItemType itemType;
    public string itemName;
    public Sprite itemSprite;

    public Item(ItemType itemType , string itemName, Sprite itemSprite)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemSprite = itemSprite;
    }
}

public class PEA_ItemSlot : MonoBehaviour
{
    public enum SlotState
    {
        Idle,
        Down,
        Action
    }

    private SlotState slotState = SlotState.Idle;

    private float clickTime = 0f;
    private float longClickTime = 0.5f;

    private GameObject newItem;

    private Item item;

    public GameObject[] itemPrefabs;
    //public GameObject imageItem;
    //public GameObject videoItem;
    //public GameObject objectItem;

    public PEA_Market market;
    public Transform canvas;

    void Start()
    {
        
    }

    void Update()
    {
        if (slotState == SlotState.Down)
        {
            clickTime += Time.deltaTime;
            if(clickTime >= longClickTime)
            {
                newItem = GetItem();
                slotState = SlotState.Action;
            }
        }
        else if( slotState == SlotState.Action)
        {
            if(newItem != null)
            {
                newItem.transform.position = Input.mousePosition;
            }
        }
    }

    public void SetItemInfo(Item item)
    {
        this.item = item;
    }

    public void OnButtonDown()
    {
        slotState = SlotState.Down;
    }

    public void OnButtonUp()
    {
        slotState = SlotState.Idle;
        clickTime = 0f;
        newItem = null;
    }

    public GameObject GetItem()
    {
        GameObject getItem = Instantiate(itemPrefabs[(int)item.itemType]);
        getItem.transform.parent = canvas;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                getItem.GetComponent<PEA_ImageItem>().SetImage(item.itemSprite);
                break;
            case Item.ItemType.Video:
                break;
            case Item.ItemType.Object:
                break;
        }

        return getItem;
    }
}
