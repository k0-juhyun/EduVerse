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

        // Scene�� �ε�� �Ŀ� ����� �ڵ�
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // �� �ڵ�� PhotonNetwork�� ����Ǿ� �ְ� �غ�� ���¿����� ����˴ϴ�.
            // �� �ڵ�� ���� �ε�� �Ŀ� ����˴ϴ�.
            NetworkManager.instance.photontext.text = "SCene�ε�";
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
