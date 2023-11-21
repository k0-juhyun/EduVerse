using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class Market : MonoBehaviour
{
    public static Market instance = null;

    public GameObject previewPanel;
    public GameObject itemView;
    public GameObject categories;

    public Image previewImage;
    public RawImage previewVideo_RawImaege;

    public VideoPlayer videoPlayer;

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
                previewImage.gameObject.SetActive(true);
                GifLoad gifLoad = previewImage.GetComponent<GifLoad>();
                (Sprite[], float) gifInfo = gifLoad.GetSpritesByFrame(item.itemPath);
                gifLoad.Show(previewImage, gifInfo.Item1, gifInfo.Item2);
                previewImage.preserveAspect = true;
                break;
            case Item.ItemType.Video:
                videoPlayer.url = item.itemPath;
                previewVideo_RawImaege.gameObject.SetActive(true);
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
        previewVideo_RawImaege.gameObject.SetActive(false);
        previewImage.GetComponent<GifLoad>().StopGif();
    }
}
