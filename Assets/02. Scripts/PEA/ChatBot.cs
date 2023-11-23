using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ChatBot : MonoBehaviour
{
    private Color answerChatColor = new Color(1f, 1f, 0.8f, 0.5f);

    public TMP_InputField questionInput;
    public GameObject answerChatItem;
    public GameObject questionChatItem;
    public Button sendBtn;

    public Transform content;

    public TextSender textSender;

    void Start()
    {
        questionInput.onSubmit.AddListener((s) => OnClickQuestionSendBtn(s));
        sendBtn.onClick.AddListener(() => OnClickQuestionSendBtn(questionInput.text));

        //AddChatText(true, "안녕");
        //AddChatText(false, "하이");
    }

    void Update()
    {
        
    }

    public void OnClickQuestionSendBtn(string text)
    {
        if (text.Length == 0)
            return;

        AddChatText(true, text);
        questionInput.text = "";
        questionInput.placeholder.GetComponent<TMP_Text>().text = "잠시만 기다려주세요!";
        questionInput.interactable = false;

        textSender?.SendText(text, () => 
        {
            questionInput.placeholder.GetComponent<TMP_Text>().text = "내용을 입력해주세요...";
            questionInput.interactable = true;
        },
        (DownloadHandler downloadHandler) =>
        {
            AddChatText(false, downloadHandler.text);
        });

        //HttpInfo httpInfo = new HttpInfo();
        //httpInfo.Set(RequestType.POST, "", (DownloadHandler downloadHandler) =>
        //{
        //    AddChatText(false, downloadHandler.text);
        //}
        //, true);
        //httpInfo.body = text;
        //PEA_HttpManager.instance.SendRequest(httpInfo);


    }

    public void AddChatText(bool isQuestion, string textContent)
    {
        GameObject chat = Instantiate(isQuestion ? questionChatItem : answerChatItem, content);
        chat.GetComponent<ChatItem>().SetText(textContent);
    }
}
