using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public HorizontalLayoutGroup horizontalgroup;
    public RectTransform ItemMenu;
    bool bool_open_menu;
    bool bool_item_menu;

    private void Start()
    {
        
    }

    // ��ư �Լ�

    // �޴�����
    public void OpenMenu()
    {
        Debug.Log("����");
        // ����������
        if (!bool_open_menu)
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, 200, 0.5f);
            bool_open_menu = !bool_open_menu;
        }
        else
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, -100, 0.5f);
            bool_open_menu = !bool_open_menu;
        }
    }

    // ������ â ����
    public void ItemOpenMenu()
    {
        Debug.Log("item");
        if (!bool_item_menu)
        {
            ItemMenu.DOMoveY(-150f, 1);
            bool_item_menu = !bool_item_menu;
        }
        else
        {
            ItemMenu.DOMoveY(100, 1);
            bool_item_menu = !bool_item_menu;
        }
    }


}
