using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionMakeBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject deleteImage; // 삭제 이미지
    [SerializeField] private GameObject rectImage;
    private bool isClick = false;
    private Vector3 pointerDownPos;
    private float pointerDownTimer = 0f;
    private const float requiredHoldTime = 3f; // 필요한 터치 시간
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

    // 아이템 선택할 수 있는 창 띄우기
    public void ShowItemList(bool isShow)
    {
        itemList.SetActive(isShow);
    }

    // 선택된 아이템 정보 저장
    public void SelectItem(Item item)
    {
        this.item = item;
        itemList.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        pointerDownTimer = 0f;
        StopAllCoroutines(); // 터치 추적 중단

        if (deleteImageActivated && RectTransformUtility.RectangleContainsScreenPoint(deleteImage.GetComponent<RectTransform>(), eventData.position, null))
        {
            Delete(); // 오브젝트 삭제
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
            deleteImage.SetActive(true); // 삭제 이미지 활성화
            deleteImageActivated = true;
        }
    }
}
