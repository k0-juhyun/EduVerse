using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Market : MonoBehaviour
{
    public static Market instance = null;

    public GameObject previewPanel;
    public GameObject itemView;
    public GameObject categories;

    public Image previewImage;
    public RawImage previewVideo_RawImaege;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        previewImage.preserveAspect = true;
    }

    void Update()
    {
        
    }

    public void Preview(Item item)
    {
        previewPanel.SetActive(true);
        itemView.SetActive(false);
        categories.SetActive(false);

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                previewImage.gameObject.SetActive(true);

                byte[] bytes = File.ReadAllBytes(item.itemPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                texture.Apply();
                previewImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                break;
            case Item.ItemType.GIF:
                GifLoad gifLoad = previewImage.GetComponent<GifLoad>();
                gifLoad.Show(previewImage, gifLoad.GetSpritesByFrame(item.itemPath));
                previewImage.preserveAspect = true;
                previewImage.gameObject.SetActive(true);
                break;
            case Item.ItemType.Video:

                break;
            case Item.ItemType.Object:
                break;
        }
    }

    public void OnClickPreviewBackBtn()
    {
        previewPanel.SetActive(false);
        itemView.SetActive(true);
        categories.SetActive(true);

        previewImage.gameObject.SetActive(false);
        previewImage.GetComponent<GifLoad>().StopGif();
    }
}
