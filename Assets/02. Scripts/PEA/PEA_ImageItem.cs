using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PEA_ImageItem : MonoBehaviour
{
    private bool isMoving = false;
    private bool isScaling = false;

    private Vector3 p;

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
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            rectTransform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z)) - p;
        }
        else
        {
            rectTransform.position = Input.mousePosition - p;
        }
    }

    public void ImageDown()
    {
        print("image down");
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            p = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z)) - rectTransform.position;
        }
        else
        {
            p = Input.mousePosition - rectTransform.position;
        }
        isMoving = true;
    }

    public void ImageUp()
    {
        print("image up");
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
