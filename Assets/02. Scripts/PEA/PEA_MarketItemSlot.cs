using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_MarketItemSlot : MonoBehaviour
{
    private Item item;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetItemInfo(Item item)
    {
        this.item = item;
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
        File.WriteAllBytes(Application.persistentDataPath + "/MyItems_Sprites/" + item.itemName + ".jpg", bytes);
        DestroyImmediate(texture);
    }
}
