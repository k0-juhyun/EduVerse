using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
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
    public string itemPath;
    public bool isMine = false;
    public bool showInClassroom = false;
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

    private string myItemsJsonPath;
    private MyItems myItems;

    private Toggle selectToggle;

    private GameObject newItem;

    private Item item;

    public GameObject[] itemPrefabs;

    public Transform canvas;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 && item!= null)
        {
            gameObject.SetActive(item.showInClassroom);
            selectToggle.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        myItemsJsonPath = Application.persistentDataPath + "/MyItems.txt";

        selectToggle = GetComponentInChildren<Toggle>();
        selectToggle.onValueChanged.AddListener((b) => OnSelectToggleValueChanged(b));
        selectToggle.isOn = item.showInClassroom;

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            selectToggle.gameObject.SetActive(false);
        }
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

    // 아이템 정보 세팅
    public void SetItemInfo(Item item)
    {
        this.item = item;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(File.ReadAllBytes(item.itemPath));
                texture.Apply();
                GetComponentInChildren<RawImage>().texture = texture;
                break;
        }

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            gameObject.SetActive(item.showInClassroom);
        }
    }

    public void OnSelectToggleValueChanged(bool isSelected)
    {
        item.showInClassroom = isSelected;

        string json;

        if (!File.Exists(myItemsJsonPath))
        {
            myItems = new MyItems();
            myItems.data = new List<Item>();
            myItems.data.Add(item);
            json = JsonUtility.ToJson(myItems);
            print(json);
        }
        else
        {
            byte[] bytes = File.ReadAllBytes(myItemsJsonPath);
            json = Encoding.UTF8.GetString(bytes);
            myItems = JsonUtility.FromJson<MyItems>(json);
            foreach (Item item in myItems.data)
            {
                if (item.itemName.Equals(this.item.itemName))
                {
                    item.showInClassroom = isSelected;
                    print(item.itemName);
                }
            }
            json = JsonUtility.ToJson(myItems);
            print(json);
        }
        File.WriteAllText(myItemsJsonPath, json);
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
