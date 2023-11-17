using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionMakeBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Item item;
    private Button btn;
    private bool isClick = false;

    private Vector3 pointerDownPos;

    public GameObject itemList;
    public GameObject itemPrefab;
    public Transform itemList_Content;
    public Button deleteBtn;

    public Item Item
    {
        set 
        {
            item = MyItemsManager.instance.GetItemInfo(value.itemPath); 
        }
        get { return item; }
    }

    void Start()
    {
        //btn = GetComponent<Button>();
        //btn.onClick.AddListener(ShowItemList);
        deleteBtn.onClick.AddListener(Delete);

        SetItemLIst();
    }

    void Update()
    {
        
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
        deleteBtn.gameObject.SetActive(!isShow);
    }

    // 선택된 아이템 정보 저장
    public void SelectItem(Item item)
    {
        this.item = item;
        itemList.SetActive(false);
        deleteBtn.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isClick && transform.position == pointerDownPos)
        {
            ShowItemList(!itemList.activeSelf);
        }

        isClick = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject == eventData.pointerCurrentRaycast.gameObject)
        {
            pointerDownPos = transform.position;
            isClick = true;
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
