using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Collections;

public class VideoCreator : MonoBehaviour
{
    private string serverURL = "http://221.163.19.218:5052/text_2_video/sendvideo";

    public GameObject capturePreview;
    public GameObject captureResultText;

    private void Start()
    {

    }

    public void UploadImageAndDownloadVideo(string imagePath)
    {
        StartCoroutine(UploadAndDownloadCoroutine(imagePath));
    }

    IEnumerator UploadAndDownloadCoroutine(string imagePath)
    {
        //string imagePath = "Assets/2.jpg";  // ���ε��� �̹��� ���� ���
        //string videoId = "your_video_id";  // ������ ID

        // �̹��� ���ε�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(imagePath), "image.png", "image/png");
        using (UnityWebRequest imageUploadRequest = UnityWebRequest.Post(serverURL, form))
        {
            yield return imageUploadRequest.SendWebRequest();
            if (imageUploadRequest.result == UnityWebRequest.Result.Success)
            {
                byte[] videoData = imageUploadRequest.downloadHandler.data;
                string videoPath = Application.persistentDataPath + "/GIF/" + Time.time + ".gif";  // ������ ������ ���� ���

                if(!Directory.Exists(Application.persistentDataPath + "/GIF/"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/GIF/");
                }

                File.WriteAllBytes(videoPath, videoData);

                // ������ ���� ����
                Item item = new Item(Item.ItemType.Video, Time.time.ToString(), videoPath);
                string json = "";
                MyItems myItems = new MyItems();

                if(File.Exists(Application.persistentDataPath + "/MyItems.txt"))
                {
                    byte[] jsonBytes = File.ReadAllBytes(Application.persistentDataPath + "/MyItems.txt");
                    json = Encoding.UTF8.GetString(jsonBytes);
                    myItems = JsonUtility.FromJson<MyItems>(json);
                }
                            
                myItems.data.Add(item);
                json = JsonUtility.ToJson(myItems);
                File.WriteAllText(Application.persistentDataPath + "/MyItems.txt", json);

                capturePreview.SetActive(false);
                captureResultText.GetComponent<Text>().text = "���������� �����߽��ϴ�.";
                System.Action action = () => { captureResultText.SetActive(false); };
                Invoke(nameof(action), 0.5f);
            }
            else
            {
                Debug.Log(imageUploadRequest.error);

                capturePreview.SetActive(false);
                captureResultText.GetComponent<Text>().text = "���忡 �����߽��ϴ�.";
                System.Action action = () => { captureResultText.SetActive(false); };
                Invoke(nameof(action), 0.5f);
            }
        }
    }
}