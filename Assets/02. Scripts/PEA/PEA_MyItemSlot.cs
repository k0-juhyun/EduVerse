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
    public Texture2D itemTexture;

    public Item(ItemType itemType , string itemName, Texture2D itemSprite)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemTexture = itemSprite;
    }
}

public class PEA_MyItemSlot : MonoBehaviour
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
                newItem = UseItem();
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

    public GameObject UseItem()
    {
        GameObject useItem = Instantiate(itemPrefabs[(int)item.itemType]);
        useItem.transform.parent = canvas;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                useItem.GetComponent<PEA_ImageItem>().SetImage(item.itemTexture);
                break;
            case Item.ItemType.Video:
                break;
            case Item.ItemType.Object:
                break;
        }

        return useItem;
    }
}
