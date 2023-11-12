using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifItem : MonoBehaviour
{
    void Start()
    {
        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
        string json = Encoding.UTF8.GetString(bytes);
        MyItems myItems = JsonUtility.FromJson<MyItems>(json);

        Item item = new Item(Item.ItemType.GIF, "24.56843", Application.persistentDataPath + "/24.56843.gif");
        myItems.data.Add(item);

        json = JsonUtility.ToJson(myItems);
        bytes = Encoding.UTF8.GetBytes(json);
        File.WriteAllBytes(Application.persistentDataPath + "/MyItems.txt" , bytes);
    }

    void Update()
    {
        
    }
}
