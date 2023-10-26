using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_ImageItem : MonoBehaviour
{
    private bool isMoving = false;
    private bool isScaling = false;
    private float moveX = 0f;
    private float moveY = 0f;

    private RectTransform rectTransform;
    private Transform imageTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageTransform = transform.GetChild(0);

        rectTransform.anchoredPosition = Vector2.zero;
        transform.localScale = Vector3.one;
        //transform.localPosition = Vector3.one;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveItem();
        }
        else if (isScaling)
        {
            SetScale();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            rectTransform.anchoredPosition = Vector2.zero;
            transform.localScale = Vector3.one;
        }
    }

    public void SetImage(Texture2D texture)
    {
        Image image = transform.GetChild(0).GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.preserveAspect = true;
        image.SetNativeSize();
        image.transform.localScale = Vector3.one * 0.1f;

        RectTransform imageRectTransform = image.GetComponent<RectTransform>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = (imageRectTransform.sizeDelta) / 10 + Vector2.one * 15f;
        rectTransform.sizeDelta = size;

        imageRectTransform.anchoredPosition = new Vector2(imageRectTransform.rect.width / 10f, imageRectTransform.rect.height / 10f);
    }

    public void MovePivot()
    {
        GetComponent<RectTransform>().pivot = Vector2.zero;
    }

    public void SetScale()
    {
        imageTransform.localScale += Vector3.one * Input.GetAxis("Mouse X") * 0.1f;
    }

    public void MoveItem()
    {
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");

        //rectTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0,0,Camera.main.transform.position.z);
    }

    public void ImageDown()
    {
        isMoving = true;
    }

    public void ImageUp()
    {
        isMoving = false;
    }

    public void ScaleDown()
    {
        isScaling = true;
    }

    public void ScaleUp()
    {
        isScaling = false;
    }

    public void OnClickDeleteBtn()
    {
        Destroy(gameObject);
    }
}
