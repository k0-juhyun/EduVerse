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
        GIF,
        Video,
        Object
    }

    public enum LoaderType
    {
        Market,
        MyItem,
        InteractionItemList
    }

    //private string myItemsJsonPath;
    private MyItems myItems;

    public LoaderType loaderType;

    private string[] gifItemsPath;
    private string[] videoItemsPath;

    private List<Texture2D> itemTextures = new List<Texture2D>();

    public Button[] searchTypeButtons;
    public GameObject itemSlot;
    public Transform content;
    public bool isMarket = false;

    //public GameObject gifMarketItem;

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
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }

        switch ((SearchType)loadItemType)
        {
            case SearchType.Image:
                itemTextures.Clear();

                List<Item> gifItems = new List<Item>();
                List<Item> videoItems = new List<Item>();
                MyItems myItems = MyItemsManager.instance.GetMyItems();

                if(loaderType == LoaderType.InteractionItemList)
                {
                    for (int i = 0; i < (myItems != null ? myItems.data.Count : 0); i++)
                    {
                        GameObject slot = Instantiate(itemSlot, content);

                        Interaction_Item  Interaction_Item  = slot.GetComponent<Interaction_Item>();
                    }
                }
                else
                {
                    if (isMarket)
                    {
                        itemTextures = Resources.LoadAll<Texture2D>("Market_Item_Sprites").ToList();

                        if (Directory.Exists(Application.persistentDataPath + "/GIF/"))
                        {
                            gifItemsPath = Directory.GetFiles(Application.persistentDataPath + "/GIF/");

                            foreach (string path in gifItemsPath)
                            {
                                Item item = new Item(Item.ItemType.GIF, Path.GetFileNameWithoutExtension(path), path);
                                gifItems.Add(item);
                            }
                        }

                        if (Directory.Exists(Application.persistentDataPath + "/Videos/"))
                        {
                            videoItemsPath = Directory.GetFiles(Application.persistentDataPath + "/Videos/");

                            foreach (string path in videoItemsPath)
                            {
                                Item item = new Item(Item.ItemType.Video, Path.GetFileNameWithoutExtension(path), path);
                                videoItems.Add(item);
                            }
                        }
                    }
                    else
                    {
                        //if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
                        //{
                        //    byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
                        //    string json = Encoding.UTF8.GetString(bytes.ToArray());
                        //    this.myItems = JsonUtility.FromJson<MyItems>(json);                        

                        //    foreach (Item item in this.myItems.data)
                        //    {
                        //        //if(item.itemType == Item.ItemType.Image)
                        //        {
                        //            //print(item.itemName + " 은 이미지");
                        //            print(item.itemName + ", " + item.showInClassroom);
                        //            myItems.data.Add(item);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    print("else");
                        //}
                    }

                    for (int i = 0; i < (isMarket ? itemTextures.Count : myItems != null ? myItems.data.Count : 0); i++)
                    {
                        GameObject slot = Instantiate(itemSlot, content);
                        if (isMarket)
                        {
                            PEA_MarketItemSlot marketItemSlot = slot.GetComponent<PEA_MarketItemSlot>();
                            marketItemSlot.SetItemInfo(new Item(Item.ItemType.Image, itemTextures[i].name, Application.dataPath + "/Resources/Market_Item_Sprites/" + itemTextures[i].name + ".jpg"));
                        }
                        else
                        {
                            PEA_MyItemSlot myItemSlot = slot.GetComponent<PEA_MyItemSlot>();
                            myItemSlot.SetItemInfo(myItems.data[i]);
                            myItemSlot.canvas = transform.parent;
                        }
                    }

                    if (isMarket)
                    {
                        if (gifItems.Count > 0)
                        {
                            for (int i = 0; i < gifItems.Count; i++)
                            {
                                GameObject slot = Instantiate(itemSlot, content);
                                slot.GetComponent<PEA_MarketItemSlot>().SetItemInfo(gifItems[i]);
                            }
                        }

                        if (videoItems.Count > 0)
                        {
                            for (int i = 0; i < videoItems.Count; i++)
                            {
                                GameObject slot = Instantiate(itemSlot, content);
                                slot.GetComponent<PEA_MarketItemSlot>().SetItemInfo(videoItems[i]);
                            }
                        }
                    }
                }                

                break;
            case SearchType.GIF:
                itemTextures.Clear();

                MyItems myGIFItems = new MyItems();
                myGIFItems.data = new List<Item>();

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
                        this.myItems = JsonUtility.FromJson<MyItems>(json);

                        foreach (Item item in this.myItems.data)
                        {
                            print(item.itemName + " : " + item.itemType);
                            if (item.itemType == Item.ItemType.GIF)
                            {
                                print(item.itemName + " 은 gif");
                                myGIFItems.data.Add(item);
                            }
                        }
                    }
                    else
                    {
                        print("else");
                    }
                }

                for (int i = 0; i < (isMarket ? itemTextures.Count : myGIFItems.data.Count); i++)
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
                        myItemSlot.SetItemInfo(myGIFItems.data[i]);
                        myItemSlot.canvas = transform.parent;
                    }
                }
                break;
                
            case SearchType.Object:
                break;
        }
    }

    public void DeleteAllMyItems()
    {
        if(File.Exists(Application.persistentDataPath + "/MyItems.txt"))
        {
            print("file exists");
            File.Delete(Application.persistentDataPath + "/MyItems.txt");

            foreach(Transform tr in content)
            {
                Destroy(tr.gameObject);
            }
        }
        MyItemsManager.instance.DeleteAll();
    }
}
