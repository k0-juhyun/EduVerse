using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionMakeBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject deleteImage; // ���� �̹���
    [SerializeField] private GameObject rectImage;
    private bool isClick = false;
    private Vector3 pointerDownPos;
    private float pointerDownTimer = 0f;
    private const float requiredHoldTime = 3f; // �ʿ��� ��ġ �ð�
    private bool isDragging = false;
    private bool deleteImageActivated = false;
    private LoadButton loadButton;

    public GameObject itemList;
    public GameObject itemPrefab;
    public Transform itemList_Content;
    public Button selectItemBtn;

    public Item Item
    {
        set 
        {
            item = MyItemsManager.instance.GetItemInfo(value.itemPath); 
        }
        get { return item; }
    }

    private void Awake()
    {
        loadButton = GetComponentInParent<LoadButton>();
    }

    void Start()
    {
        //btn = GetComponent<Button>();
        //btn.onClick.AddListener(ShowItemList);

        deleteImage = loadButton.trashCan;
        rectImage = loadButton.trashCan.transform.GetChild(0).gameObject;
        selectItemBtn.onClick.AddListener(() => ShowItemList(!itemList.activeSelf));
        SetItemLIst();
    }

    private void SetItemLIst()
    {
        MyItems myItems = MyItemsManager.instance.GetMyItems();
        for (int i = 0; i < myItems.data.Count; i++)
        {
            GameObject item = Instantiate(itemPrefab, itemList_Content);
            item.GetComponent<Interaction_Item>().SetItem(MyItemsManager.instance.GetItemInfo( myItems.data[i].itemPath));
        }
    }

    // ������ ������ �� �ִ� â ����
    public void ShowItemList(bool isShow)
    {
        itemList.SetActive(isShow);
    }

    // ���õ� ������ ���� ����
    public void SelectItem(Item item)
    {
        this.item = item;
        itemList.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        pointerDownTimer = 0f;
        StopAllCoroutines(); // ��ġ ���� �ߴ�

        if (deleteImageActivated && RectTransformUtility.RectangleContainsScreenPoint(deleteImage.GetComponent<RectTransform>(), eventData.position, null))
        {
            Delete(); // ������Ʈ ����
        }
        deleteImageActivated = false;
        deleteImage.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject == eventData.pointerCurrentRaycast.gameObject)
        {
            isClick = true;
            pointerDownPos = transform.position;
            StartCoroutine(TrackPointerDownTime());
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private IEnumerator TrackPointerDownTime()
    {
        while (isClick && pointerDownTimer < requiredHoldTime)
        {
            pointerDownTimer += Time.deltaTime;
            yield return null;
        }

        if (pointerDownTimer >= requiredHoldTime)
        {
            deleteImage.SetActive(true); // ���� �̹��� Ȱ��ȭ
            deleteImageActivated = true;
        }
    }
}
