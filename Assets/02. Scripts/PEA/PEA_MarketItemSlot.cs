using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PEA_MarketItemSlot : MonoBehaviour
{
    private string myItemsDataPath;
    private string myItemsJsonPath;

    public VideoPlayer videoPlayer;
    public Item item;

    private MyItems myItems;

    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
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
                //GetComponentInChildren<RawImage>().texture = item.itemTexture;
                break;
            case Item.ItemType.GIF:
                byte[] gifBytes = File.ReadAllBytes(Application.persistentDataPath + "/GIFThumbNails/" + Path.GetFileNameWithoutExtension(item.itemPath) + ".png");
                Texture2D thumbNailTexture = new Texture2D(2, 2);
                thumbNailTexture.LoadImage(gifBytes);
                thumbNailTexture.Apply();
                GetComponentInChildren<RawImage>().texture = thumbNailTexture;
                //GetComponentInChildren<RawImage>().texture = GetComponent<GifLoad>().GetSpritesByFrame(item.itemPath).Item1[0].texture;
                break;
            case Item.ItemType.Video:
                videoPlayer.url = item.itemPath;
                videoPlayer.targetTexture = new RenderTexture(videoPlayer.targetTexture);
                GetComponentInChildren<RawImage>().texture = videoPlayer.targetTexture;
                break;
        }

    }

    public void ItemPreview()
    {
        Market.instance.Preview(item);
    }

    public void OnClickAddMyItem()
    {
        MyItemsManager.instance.AddItem(item);
        //StartCoroutine(IOnClickAddMyItem());
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
