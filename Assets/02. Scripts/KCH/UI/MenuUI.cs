using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor.Build;

public class MenuUI : MonoBehaviour
{
    public HorizontalLayoutGroup horizontalgroup;
    public RectTransform ItemMenu;
    public RectTransform ChatBotMenu;

    bool bool_open_menu;
    bool bool_item_menu;
    bool bool_chatbot_menu;

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
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, 200, 0.2f).SetEase(Ease.OutBack);
            bool_open_menu = !bool_open_menu;
        }
        else
        {
            DOTween.To(() => horizontalgroup.spacing, x => horizontalgroup.spacing = x, -100, 0.2f);
            bool_open_menu = !bool_open_menu;
        }
    }

    // ������ â ����
    public void ItemOpenMenu()
    {
        Debug.Log("item");
        if (!bool_item_menu)
        {
            ItemMenu.DOLocalMoveY(750, 0.5f);
            bool_item_menu = !bool_item_menu;
        }
        else
        {
            ItemMenu.DOLocalMoveY(1000, 0.5f);
            bool_item_menu = !bool_item_menu;
        }
    }

    // ê �� ����
    public void ChatBotMeny()
    {
        Debug.Log("item");
        if (!bool_item_menu)
        {
            ChatBotMenu.DOLocalMoveY(750, 0.5f);
            bool_item_menu = !bool_item_menu;
        }
        else
        {
            ChatBotMenu.DOLocalMoveY(1000, 0.5f);
            bool_item_menu = !bool_item_menu;
        }
    }
}
