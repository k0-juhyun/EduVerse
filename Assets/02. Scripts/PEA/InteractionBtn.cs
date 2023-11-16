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

    // ������ ������ �� �ִ� â ����
    public void ShowItemList()
    {
        itemList.SetActive(true);
    }

    // ���õ� ������ ���� ����
    public void SelectItem(Item item)
    {
        this.item = item;
        itemList.SetActive(false);
    }
}
