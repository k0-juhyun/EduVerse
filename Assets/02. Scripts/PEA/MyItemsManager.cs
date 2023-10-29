using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemsManager : MonoBehaviour
{
    public static MyItemsManager instance = null;

    private MyItems myItems;

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

            print(json);
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

        foreach(Item myItem in myItems.data)
        {
            if (myItem.itemPath.Equals(item.itemPath))
            {
                return;
            }
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
