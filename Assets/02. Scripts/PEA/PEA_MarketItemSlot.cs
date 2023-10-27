using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PEA_MarketItemSlot : MonoBehaviour
{
    private string myItemsDataPath;
    private string myItemsJsonPath;
    public Item item;

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
                byte[] bytes = File.ReadAllBytes(item.itemPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                texture.Apply();
                GetComponentInChildren<RawImage>().texture = texture;
                break;
            case Item.ItemType.Video:
                GetComponentInChildren<RawImage>().texture = GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath)[0].texture;
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

        if (!Directory.Exists(myItemsDataPath))
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
        }
        else
        {
            byte[] jsonBytes = File.ReadAllBytes(myItemsJsonPath);
            json = Encoding.UTF8.GetString(jsonBytes.ToArray());
            myItems = JsonUtility.FromJson<MyItems>(json);
            myItems.data.Add(item);
            json = JsonUtility.ToJson(myItems);
        }
        File.WriteAllText(myItemsJsonPath, json);

        DestroyImmediate(texture);
    }
}
