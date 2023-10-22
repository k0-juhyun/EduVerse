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
        // InputField가 비어있으면 종료
        if (inputField.text.Equals("")) return;

        // 대화 내용 출력을 위해 TextUI 생성 textChatPrefab 을 복제해 parentContent의 자식으로 배치
        GameObject clone = Instantiate(textChatPrefab, parentContent);

        // 대화 입력창에 있는 내용을 대화창에 출력
        clone.GetComponent<Text>().text = inputField.text;

        // 대화 입력창에 있는 내용 초기화
        inputField.text = "";
    }
}
