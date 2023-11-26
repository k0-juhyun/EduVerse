using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Image,
        GIF,
        Video,
        Object
    }

    public ItemType itemType;
    public string itemName;
    public string itemPath;
    public bool isMine = false;
    public bool showInClassroom = false;
    public Texture2D itemTexture;
    public Texture2D gifThumbNailTexture;
    public byte[] gifBytes;
    public Sprite[] gifSprites;
    public float gifDelayTime;

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

    public Item(ItemType itemType, string itemName, Sprite[] gifSprites)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.gifSprites = gifSprites;
    } 
    public Item(ItemType itemType, string itemName, Sprite[] gifSprites, string itemPath)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.gifSprites = gifSprites;
        this.itemPath = itemPath;
    }

    public Item(ItemType itemType, string itemName, byte[] itemBytes)
    {
        this.itemName = itemName;

        switch (itemType) 
        {
            case ItemType.Image:
                itemTexture.LoadImage(itemBytes);
                itemTexture.Apply();
                break;

            case ItemType.GIF:
                break;

            case ItemType.Video:
                itemPath = Application.persistentDataPath + "/Videos/" + itemName + ".mp4";
                if(!Directory.Exists(Application.persistentDataPath + "/Videos/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/Videos/");
                }
                File.WriteAllBytes(itemPath, itemBytes);
                break;
        }
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

    public Toggle selectToggle;

    private GameObject newItem;

    public VideoPlayer videoPlayer;

    [SerializeField]
    private Item item;

    public GameObject[] itemPrefabs;

    public Transform canvas;
    public GameObject toggleIsOnBG;

    private bool isShowInClassroom;

    public Item Item
    {
        get { return item; }
    }

    private void OnEnable()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 4 && item!= null)
        //{
        //    print("slot, " + item.itemName + ", " + item.showInClassroom);
        //    gameObject.SetActive(item.showInClassroom);
        //    selectToggle.gameObject.SetActive(false);
        //}
    }

    void Start()
    {
        myItemsJsonPath = Application.persistentDataPath + "/MyItems.txt";

        //selectToggle = GetComponentInChildren<Toggle>();
        //selectToggle.onValueChanged.AddListener((b) => OnSelectToggleValueChanged(b));
        selectToggle.isOn = item.showInClassroom;
        toggleIsOnBG.SetActive(selectToggle.isOn);

        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            selectToggle.gameObject.SetActive(false);
            toggleIsOnBG.SetActive(false);
        }

        GetComponentInChildren<Button>().onClick.AddListener(OnClickBtn);
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
                print("action");
                if(SceneManager.GetActiveScene().buildIndex == 4)
                {
                    newItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
                }
                else
                {
                    newItem.transform.position = Input.mousePosition;
                }
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
            case Item.ItemType.GIF:
                //GetComponentInChildren<RawImage>().texture = GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath).Item1[0].texture;
                GetComponentInChildren<RawImage>().texture = item.gifThumbNailTexture;
                break;
            case Item.ItemType.Video:
                videoPlayer.url = item.itemPath;
                videoPlayer.targetTexture = new RenderTexture(videoPlayer.targetTexture);
                GetComponentInChildren<RawImage>().texture = videoPlayer.targetTexture;
                break;
        }

        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            isShowInClassroom = item.showInClassroom;
            gameObject.SetActive(isShowInClassroom);
            selectToggle.gameObject.SetActive(false);
            toggleIsOnBG.SetActive(false);
        }
    }

    public void OnClickBtn()
    {
        isShowInClassroom = !isShowInClassroom;
        selectToggle.isOn = isShowInClassroom;
        toggleIsOnBG.SetActive(isShowInClassroom);

        item.showInClassroom = isShowInClassroom;
        MyItemsManager.instance.EditItemData(item);
    }

    public void OnSelectToggleValueChanged(bool isSelected)
    {
        item.showInClassroom = isSelected;

        MyItemsManager.instance.EditItemData(item);

        //string json;

        //if (!File.Exists(myItemsJsonPath))
        //{
        //    myItems = new MyItems();
        //    myItems.data = new List<Item>();
        //    myItems.data.Add(item);
        //    json = JsonUtility.ToJson(myItems);
        //    print(json);
        //}
        //else
        //{
        //    byte[] bytes = File.ReadAllBytes(myItemsJsonPath);
        //    json = Encoding.UTF8.GetString(bytes);
        //    myItems = JsonUtility.FromJson<MyItems>(json);
        //    foreach (Item item in myItems.data)
        //    {
        //        if (item.itemName.Equals(this.item.itemName))
        //        {
        //            item.showInClassroom = isSelected;
        //        }
        //    }
        //    json = JsonUtility.ToJson(myItems);
        //}
        //File.WriteAllText(myItemsJsonPath, json);
        toggleIsOnBG.SetActive(selectToggle.isOn);
    }

    public void OnButtonDown()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            slotState = SlotState.Down;
        }
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
        //GameObject useItem;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                byte[] bytes =  File.ReadAllBytes(item.itemPath);

                Texture2D texture = new Texture2D(2,2);
                texture.LoadImage(bytes);
                texture.Apply();
                useItem = Instantiate(itemPrefabs[0]);
                useItem.GetComponent<PEA_ImageItem>().SetImage(texture);
                break;
            case Item.ItemType.GIF:
                useItem = Instantiate(itemPrefabs[0]);
                (Sprite[], float) gifInfo = useItem.GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath);
                useItem.GetComponent<GifLoad>().Show(useItem.GetComponentInChildren<Image>(), gifInfo.Item1, gifInfo.Item2);
                useItem.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
                break;
            case Item.ItemType.Video:
                useItem = Instantiate(itemPrefabs[1]);
                useItem.GetComponent<VideoPlayer>().url = item.itemPath;
                break;
            case Item.ItemType.Object:
                break;
        }

        useItem.transform.parent = canvas;
        return useItem;
    }
}
