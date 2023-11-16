using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemsManager : MonoBehaviour
{
    public static MyItemsManager instance = null;

    private MyItems myItems;
    private GifLoad gifload;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gifload = GetComponent<GifLoad>();
    }

    void Update()
    {
        
    }

    public void  LoadData()
    {
        print("LoadData");
        if (File.Exists(Application.persistentDataPath + "/MyItems.txt"))
        {
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
            print(bytes.Length);
            string json = Encoding.UTF8.GetString(bytes);
            myItems = JsonUtility.FromJson<MyItems>(json);
        }

        for (int i = 0; i < myItems.data.Count; i++)
        {
            switch (myItems.data[i].itemType)
            {
                case Item.ItemType.Image:
                    byte[] imageBytes = File.ReadAllBytes(myItems.data[i].itemPath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageBytes);
                    texture.Apply();
                    myItems.data[i].itemTexture = texture;
                    break;
                case Item.ItemType.GIF:
                    byte[] gifBytes = File.ReadAllBytes(myItems.data[i].itemPath);
                    myItems.data[i].gifSprites = gifload.GetSpritesByFrame(gifBytes);
                    break;
                case Item.ItemType.Video:
                    break;
                case Item.ItemType.Object:
                    break;
                default:
                    break;
            }
        }
    }

    public MyItems GetMyItems()
    {
        return myItems;
    }

    public void AddItem(Item item)
    {
        if(myItems == null)
        {
            myItems = new MyItems();
            myItems.data = new List<Item>();
        }

        // 아이템 주소값 비교해서 같은 아이템 중복으로 담지 않음
        foreach(Item myItem in myItems.data)
        {
            if (myItem.itemPath.Equals(item.itemPath))
            {
                return;
            }
        }

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                byte[] imageBytes = File.ReadAllBytes(item.itemPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                texture.Apply();
                item.itemTexture = texture;
                break;
            case Item.ItemType.GIF:
                byte[] gifBytes = File.ReadAllBytes(item.itemPath);
                item.gifSprites =  gifload.GetSpritesByFrame(gifBytes);
                break;
            case Item.ItemType.Video:
                break;
            case Item.ItemType.Object:
                break;
            default:
                break;
        }

        myItems.data.Add(item);
        SaveData();
    }

    public void DeleteItem(Item item)
    {
        foreach(Item myItem in myItems.data)
        {
            if (myItem.itemPath.Equals(item.itemPath))
            {
                myItems.data.Remove(myItem);
            }
        }
        SaveData();
    }

    public void EditItemData(Item item)
    {
        for(int i = 0; i < myItems.data.Count; i++)
        {
            if(myItems.data[i].itemPath == item.itemPath)
            {
                myItems.data[i] = item;
                break;
            }
        }

        SaveData();
    }

    public void DeleteAll()
    {
        myItems.data.Clear();
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(myItems);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        File.WriteAllBytes(Application.persistentDataPath + "/MyItems.txt", bytes);
    }
}
