using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    public Text chatText;

    public void SetText(string text)
    {
        this.chatText.text = text;
    }
}
