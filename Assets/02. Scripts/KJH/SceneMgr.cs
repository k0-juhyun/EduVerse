using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneMgr : MonoBehaviour
{
    public Button Btn_LoadScene;

    private void Awake()
    {
        Btn_LoadScene.onClick.AddListener(() => OnLoadNextSceneClick());

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLoadNextSceneClick()
    {
        SceneManager.LoadScene("ClassRoomScene");
    }
}
