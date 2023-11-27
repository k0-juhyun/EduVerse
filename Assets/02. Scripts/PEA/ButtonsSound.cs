using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSound : MonoBehaviour
{
    public Button[] buttons;
    //public Button[] buttons2;

    void Start()
    {
        foreach(Button btn in buttons)
        {
            btn.onClick?.AddListener(() => SoundManager.instance?.PlaySFX(SoundManager.SFXClip.Button1));
        }
        //foreach(Button btn in buttons2)
        //{
        //    btn.onClick?.AddListener(() => SoundManager.instance?.PlaySFX(SoundManager.SFXClip.Button2));
        //}
    }
}
