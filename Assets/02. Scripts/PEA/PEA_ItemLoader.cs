using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MyItems
{
    public List<Item> data;
}

// 마켓, 내 아이템에서 아이템 불러오는 스크립트
public class PEA_ItemLoader : MonoBehaviour
{
    public enum SearchType
    {
        Image,
        Video,
        Object
    }

    //private string myItemsJsonPath;
    private MyItems myItems;

    private List<Texture2D> itemTextures = new List<Texture2D>();

    public Button[] searchTypeButtons;
    public GameObject itemSlot;
    public Transform content;
    public bool isMarket = false;

    private void OnEnable()
    {
        LoadItems(0);
    }

    void Start()
    {
        //myItemsJsonPath = Application.persistentDataPath + "/MyItems.txt";
    }

    void Update()
    {
        
    }

    // 아이템 불러오기
    public void LoadItems(int loadItemType)
    {
        print(content.name);
        foreach (Transform tr in content)
        {
            print(tr.name);
            Destroy(tr.gameObject);
        }

        switch ((SearchType)loadItemType)
        {
            case SearchType.Image:
                itemTextures.Clear();

                MyItems imageItems = new MyItems();
                imageItems.data = new List<Item>();

                if (isMarket)
                {
                    itemTextures = Resources.LoadAll<Texture2D>("Market_Item_Sprites").ToList();
                }
                else
                {
                    if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
                    {
                        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
                        string json = Encoding.UTF8.GetString(bytes.ToArray());
                        myItems = JsonUtility.FromJson<MyItems>(json);                        

                        foreach (Item item in myItems.data)
                        {
                            //if(item.itemType == Item.ItemType.Image)
                            {
                                //print(item.itemName + " 은 이미지");
                                imageItems.data.Add(item);
                            }
                        }
                    }
                    else
                    {
                        print("else");
                    }
                }

                for (int i = 0; i < (isMarket ? itemTextures.Count : imageItems.data.Count); i++)
                {
                    GameObject slot = Instantiate(itemSlot, content);
                    if (isMarket)
                    {
                        PEA_MarketItemSlot marketItemSlot = slot.GetComponent<PEA_MarketItemSlot>();
                        marketItemSlot.SetItemInfo(new Item(Item.ItemType.Image, itemTextures[i].name, itemTextures[i]));
                    }
                    else
                    {
                        PEA_MyItemSlot myItemSlot = slot.GetComponent<PEA_MyItemSlot>();
                        myItemSlot.SetItemInfo(imageItems.data[i]);
                        myItemSlot.canvas = transform.parent;
                    }
                }
                break;
            case SearchType.Video:
                itemTextures.Clear();

                MyItems videoItems = new MyItems();
                videoItems.data = new List<Item>();

                if (isMarket)
                {
                    //itemTextures = Resources.LoadAll<Texture2D>("Market_Item_Sprites").ToList();
                }
                else
                {
                    if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
                    {
                        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
                        string json = Encoding.UTF8.GetString(bytes.ToArray());
                        myItems = JsonUtility.FromJson<MyItems>(json);

                        foreach (Item item in myItems.data)
                        {
                            print(item.itemName + " : " + item.itemType);
                            if (item.itemType == Item.ItemType.Video)
                            {
                                print(item.itemName + " 은 gif");
                                videoItems.data.Add(item);
                            }
                        }
                    }
                    else
                    {
                        print("else");
                    }
                }

                for (int i = 0; i < (isMarket ? itemTextures.Count : videoItems.data.Count); i++)
                {
                    GameObject slot = Instantiate(itemSlot, content);
                    if (isMarket)
                    {
                        PEA_MarketItemSlot marketItemSlot = slot.GetComponent<PEA_MarketItemSlot>();
                        marketItemSlot.SetItemInfo(new Item(Item.ItemType.Image, itemTextures[i].name));
                    }
                    else
                    {
                        PEA_MyItemSlot myItemSlot = slot.GetComponent<PEA_MyItemSlot>();
                        myItemSlot.SetItemInfo(videoItems.data[i]);
                        myItemSlot.canvas = transform.parent;
                    }
                }

                break;
            case SearchType.Object:
                break;
        }
    }
}
