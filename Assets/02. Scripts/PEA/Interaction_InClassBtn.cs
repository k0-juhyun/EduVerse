using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Interaction_InClassBtn : MonoBehaviour
{
    private GifLoad gifaLoad;

    public Item item;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Image:
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(item.gifBytes);
                texture.Apply();
                item.itemTexture = texture; 
                break;

            case Item.ItemType.GIF:
                item.gifSprites = gifaLoad.GetSpritesByFrame(item.gifBytes).Item1;
                break;

            case Item.ItemType.Video:
                if (Directory.Exists(Path.GetDirectoryName(item.itemPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(item.itemPath));
                }
                File.WriteAllBytes(item.itemPath, item.gifBytes);
                break;

            case Item.ItemType.Object:
                break;

            default:
                break;
        }

        this.item = item;
    }
}
