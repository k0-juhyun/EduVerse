using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Item item;
    private Button btn;

    private Vector3 pointerDownPos;

    public GameObject itemList;
    public GameObject itemPrefab;
    public Transform itemList_Content;
    public Button deleteBtn;

    public Item Item
    {
        set { item = value; }
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
            item.GetComponent<Interaction_Item>().SetItem(myItems.data[i]);
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
        if(transform.position == pointerDownPos)
        {
            ShowItemList(!itemList.activeSelf);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownPos = transform.position;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
