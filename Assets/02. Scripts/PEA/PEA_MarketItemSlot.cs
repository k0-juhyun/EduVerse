using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_MarketItemSlot : MonoBehaviour
{
    private string myItemsDataPath;
    private string myItemsJsonPath;
    private Item item;

    private MyItems myItems;

    void Start()
    {
        myItemsDataPath = Application.persistentDataPath + "/MyItems_Sprites/";
        myItemsJsonPath = Application.persistentDataPath + "/MyItems.txt";
    }

    void Update()
    {
        
    }

    public void SetItemInfo(Item item)
    {
        this.item = item;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                GetComponentInChildren<RawImage>().texture = item.itemTexture;
                break;
        }
    }

    public void ItemPreview() 
    {
        Market.instance.Preview(item);
    }

    public void OnClickAddMyItem()
    {
        StartCoroutine(IOnClickAddMyItem());

    }

    IEnumerator IOnClickAddMyItem()
    {
        yield return new WaitForEndOfFrame();

        Texture2D t = item.itemTexture;
        Texture2D texture = new Texture2D(t.width, t.height);
        texture.SetPixels(0, 0, t.width, t.height, t.GetPixels());
        byte[] bytes = texture.EncodeToJPG();

        string json;

        if(!Directory.Exists(myItemsDataPath))
        {
            Directory.CreateDirectory(myItemsDataPath);
        }

        string texturePath = myItemsDataPath + item.itemName + ".jpg";
        File.WriteAllBytes(texturePath, bytes);

        item.itemPath = texturePath;
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
            byte[] jsonBytes = File.ReadAllBytes(myItemsJsonPath);
            json = Encoding.UTF8.GetString(jsonBytes.ToArray());
            myItems = JsonUtility.FromJson<MyItems>(json);
            myItems.data.Add(item);
            json = JsonUtility.ToJson(myItems);
            print(json);
        }
        File.WriteAllText(myItemsJsonPath, json);

        DestroyImmediate(texture);
    }
}
