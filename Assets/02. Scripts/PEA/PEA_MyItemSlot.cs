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

    public Item(ItemType itemType , string itemName, string itemPath ="")
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemPath = itemPath;
    }

    public Item(ItemType itemType, string itemName, Texture2D itemTexture)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemTexture = itemTexture;
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

    [SerializeField]
    private Item item;

    public GameObject[] itemPrefabs;

    public Transform canvas;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4 && item!= null)
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
                newItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
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
            case Item.ItemType.Video:
                GetComponentInChildren<RawImage>().texture = GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath)[0].texture;
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
        //GameObject useItem = Instantiate(itemPrefabs[(int)item.itemType]);
        GameObject useItem = Instantiate(itemPrefabs[0]);
        useItem.transform.parent = canvas;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                byte[] bytes =  File.ReadAllBytes(item.itemPath);

                Texture2D texture = new Texture2D(2,2);
                texture.LoadImage(bytes);
                texture.Apply();
                useItem.GetComponent<PEA_ImageItem>().SetImage(texture);
                break;
            case Item.ItemType.Video:
                useItem.GetComponent<GifLoad>().Show(useItem.GetComponentInChildren<Image>(), useItem.GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath));
                useItem.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
                break;
            case Item.ItemType.Object:
                break;
        }

        return useItem;
    }
}
