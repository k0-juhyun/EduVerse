using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatItem : MonoBehaviour
{
    public TMP_Text chatText;

    public void SetText(string text)
    {
        this.chatText.text = text;
    }
}
