using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSound : MonoBehaviour
{
    public Button[] buttons;

    void Start()
    {
        foreach(Button btn in buttons)
        {
            btn.onClick.AddListener(() => SoundManager.instance.PlaySFX(SoundManager.SFXClip.Button));
        }
    }
}
