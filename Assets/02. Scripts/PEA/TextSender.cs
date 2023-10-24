using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TextSender : MonoBehaviour
{
    // FastAPI ���� ��������Ʈ URL
    private string serverUrl = "http://221.163.19.218:5051/chat/send"; // ���� �ּ� �� ��������Ʈ�� ������ �����ϼ���.

    // ���� �ؽ�Ʈ ������
    private string textToSend = "������� � ���� �߾�?";


    private void Start()
    {
        //SendText();
    }

    // HTTP ��û ������ �Լ�
    IEnumerator SendTextToServer(string text, System.Action<DownloadHandler> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("text", text);

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Server response: " + www.downloadHandler.text);

                if(callback != null)
                {
                    callback(www.downloadHandler);
                }
            }
        }
    }

    // Unity���� ��ư �Ǵ� �̺�Ʈ���� �� �Լ��� ȣ���Ͽ� �ؽ�Ʈ�� ������ ���� �� �ֽ��ϴ�.
    public void SendText(string text, System.Action<DownloadHandler> callback)
    {
        StartCoroutine(SendTextToServer(text, callback));
    }
}