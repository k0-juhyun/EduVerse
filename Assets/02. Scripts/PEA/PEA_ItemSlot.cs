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
    private float longClickTime = 1f;

    private GameObject newItem;
    public GameObject imageItem;
    public Item item;
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
            print("action");
            newItem.transform.position = Input.mousePosition;
        }
    }

    public void OnButtonDown()
    {
        slotState = SlotState.Down;
        print("down");
    }

    public void OnButtonUp()
    {
        slotState = SlotState.Idle;
        clickTime = 0f;
        print("up");
    }

    public GameObject GetItem()
    {
        GameObject getItem = Instantiate(imageItem);
        getItem.transform.parent = canvas;
        getItem.GetComponent<Image>().sprite  = item.itemSprite;
        getItem.GetComponent<Image>().preserveAspect = true;

        return getItem;
    }
}
