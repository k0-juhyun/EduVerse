using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject textChatPrefab;

    [SerializeField] private Transform parentContent;

    [SerializeField] private InputField inputField;

    public void OnEndEditEventMethod()
    {
        UpdateChat();
    }

    public void UpdateChat()
    {
        // InputField�� ��������� ����
        if (inputField.text.Equals("")) return;

        // ��ȭ ���� ����� ���� TextUI ���� textChatPrefab �� ������ parentContent�� �ڽ����� ��ġ
        GameObject clone = Instantiate(textChatPrefab, parentContent);

        // ��ȭ �Է�â�� �ִ� ������ ��ȭâ�� ���
        clone.GetComponent<Text>().text = inputField.text;

        // ��ȭ �Է�â�� �ִ� ���� �ʱ�ȭ
        inputField.text = "";
    }
}
