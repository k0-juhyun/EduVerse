using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public Button Btn_LoadScene;
    public Button Btn_Save;

    private void Awake()
    {
        Btn_Save.onClick.AddListener(() => GameManager.Instance.SaveCharacterInfo());
        Btn_LoadScene.onClick.AddListener(() => OnLoadNextSceneClick());
    }

    private void OnLoadNextSceneClick()
    {
        NetworkManager.instance.JoinRoom();
    }
}
