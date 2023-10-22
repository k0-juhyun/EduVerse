using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public static Market instance = null;

    public GameObject previewPanel;
    public GameObject itemView;

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

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                previewImage.gameObject.SetActive(true);
                previewImage.sprite = Sprite.Create(item.itemTexture, new Rect(0, 0, item.itemTexture.width, item.itemTexture.height), new Vector2(0.5f, 0.5f));
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

        previewImage.gameObject.SetActive(false);
    }
}
