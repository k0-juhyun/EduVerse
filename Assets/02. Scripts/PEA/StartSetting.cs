using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartSetting : MonoBehaviour
{
    public InputField nameInput;
    public Toggle isTeacherTpggle;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator LoadLevelAsync(int levelIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene이 로드된 후에 실행될 코드
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // 이 코드는 PhotonNetwork가 연결되어 있고 준비된 상태에서만 실행됩니다.
            // 이 코드는 씬이 로드된 후에 실행됩니다.
            NetworkManager.instance.photontext.text = "SCene로드";
        }
    }

    public void OnCllickStartBtn()
    {
        if (nameInput.text.Length > 0)
        {
            PlayerManager.instance.SetMyInfo(new User(nameInput.text, isTeacherTpggle.isOn));

            PhotonNetwork.LoadLevel(1);

            //StartCoroutine(LoadLevelAsync(1));
        }
    }
}
