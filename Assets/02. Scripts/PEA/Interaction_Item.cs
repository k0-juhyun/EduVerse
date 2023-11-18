using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class Interaction_Item : MonoBehaviour, IPointerClickHandler
{
    private InteractionMakeBtn interactionBtn;

    private Item item;

    public Image image;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    public Item Item
    {
        get { return item; }
    }

    void Start()
    {
        interactionBtn = GetComponentInParent<InteractionMakeBtn>();
    }

    void Update()
    {
        
    }

    public void SetItem(Item item)
    {
        this.item = item;
        switch (item.itemType)
        {
            case Item.ItemType.Image:
                image.sprite = Sprite.Create(item.itemTexture, new Rect(0, 0, item.itemTexture.width, item.itemTexture.height), Vector2.zero);
                image.preserveAspect = true;
                image.gameObject.SetActive(true);
                break;
            case Item.ItemType.GIF:
                image.sprite = item.gifSprites[0];
                image.gameObject.SetActive(true);
                break;
            case Item.ItemType.Video:
                rawImage.texture = videoPlayer.targetTexture;
                videoPlayer.url = item.itemPath;
                rawImage.gameObject.SetActive(true);
                break;
            case Item.ItemType.Object:
                break;
            default:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("cccccc");
        interactionBtn.SelectItem(item);
    }
}
