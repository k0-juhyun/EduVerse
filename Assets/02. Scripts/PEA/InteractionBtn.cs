using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBtn : MonoBehaviour
{
    private Item item;
    private Button btn;

    public GameObject itemList;

    public Item Item
    {
        get { return item; }
    }

    void Start()
    {
        btn.onClick.AddListener(ShowItemList);
    }

    void Update()
    {
        
    }

    // 아이템 선택할 수 있는 창 띄우기
    public void ShowItemList()
    {
        itemList.SetActive(true);
    }

    // 선택된 아이템 정보 저장
    public void SelectItem(Item item)
    {
        this.item = item;
        itemList.SetActive(false);
    }
}
