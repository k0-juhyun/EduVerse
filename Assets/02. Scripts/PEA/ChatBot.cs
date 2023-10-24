using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatBot : MonoBehaviour
{
    private Color answerChatColor = new Color(1f, 1f, 0.8f, 0.5f);

    public InputField questionInput;
    public GameObject chatItem;
    public Button sendBtn;

    public Transform content;

    public TextSender textSender;

    void Start()
    {
        questionInput.onSubmit.AddListener((s) => OnClickQuestionSendBtn(s));
        sendBtn.onClick.AddListener(() => OnClickQuestionSendBtn(questionInput.text));

        //AddChatText(true, "¾È³ç");
        //AddChatText(false, "ÇÏÀÌ");
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

        textSender.SendText(text, (DownloadHandler downloadHandler) =>
            {
                AddChatText(false, downloadHandler.text);
            }
        );

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
        GameObject chat = Instantiate(chatItem, content);
        Text chatText = chat.GetComponentInChildren<Text>();
        chatText.text = (isQuestion ? "³ª : " : "Ãªº¿ : ") + textContent;
       // chatText.color = isQuestion ? Color.black : Color.blue;
        chatText.fontStyle = isQuestion ? FontStyle.Normal : FontStyle.Bold;
        if (!isQuestion)
        {
            chat.GetComponent<Image>().color = answerChatColor;
        }
    }
}
