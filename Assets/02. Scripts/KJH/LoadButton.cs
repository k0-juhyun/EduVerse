using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Pun;

[System.Serializable]
public class ButtonPositionData
{
    public int page;
    public List<ButtonPosition> buttonPositions = new List<ButtonPosition>();
}

[System.Serializable]
public class ButtonPosition
{
    public string buttonName;
    public float posX, posY;
    public Item item;
}

[System.Serializable]
public class ButtonSessions
{
    public List<ButtonPositionData> sessions = new List<ButtonPositionData>();
}

public class LoadButton : MonoBehaviourPun
{
    private bool isLesson = false;        // ���������� Ȯ�� (���ǿ��� ���������� ���� �� pdf�� �Ѱܵ� ���ͷ��� ��ư ������ �ʰ� ������ ������ ������� �ִ� ���ͷ��� ��ư�� ����)
    private string json;                  // ���ͷ��� ��ư ���� json RPC�� �Ѱ��ٰ���
    private string filePath;
    private ButtonSessions allSessions;

    [Space(10)]
    public Button createBtn;
    public Button saveBtn;


    [Space (10)]
    public GameObject selectItemButtonPrefab;           // ���ͷ����� ������ �����ϴ� ��ư
    public GameObject inClassButtonPrefab;              // �����ð��� ������ ����� ��ư

    [Space(10)]
    public GameObject teachingData;

    [Space(10)]
    public GameObject showItemPanel;
    public Image showItemImage;
    public RawImage showItemRawImage;
    public GifLoad gifLoad;
    public VideoPlayer videoPlayer;
    public Button closeBtn;
    public Button lessonBtn;                          // ���� ���߸�� ��ư
    public GameObject trashCan;

    [HideInInspector] public PhotonView myPhotonView;

    [Space(10)]
    public Paroxe.PdfRenderer.PDFViewer pdfViewer;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "buttonSessions.json");
        LoadAllSessions();
    }

    private void Start()
    {
        myPhotonView = photonView;
        closeBtn?.onClick.AddListener(CloseShowItem);
        createBtn?.onClick.AddListener(OnClickCreateButton);
        saveBtn?.onClick.AddListener(SaveCurrentSession);
        lessonBtn?.onClick.AddListener(OnClickLessonBtn);
    }

    private void OnClickLessonBtn()
    {
        isLesson = !isLesson;

        if (!isLesson)
        {
            photonView.RPC( nameof(DestroyAllButtons), RpcTarget.All);
        }
    }

    public void OnClickCreateButton()
    {
        //// ���� ��ư�� �̸��� "Button"�� �ƴ϶�� �� ��ư ����
        //if (clickedButton.name == "Create")
        //{
        //    GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);
        //    newButton.AddComponent<DraggableButton>();

        //    // �� ��ư�� �̸� ���� (��: "NewButton_1", "NewButton_2", ...)
        //    newButton.name = "NewButton_" + newButton.GetInstanceID();
        //}

        // �ε�� PDF������ ������ ����.
        if (!pdfViewer.IsLoaded)
            return;

        GameObject newButton = Instantiate(selectItemButtonPrefab, teachingData.transform);

        // �� ��ư�� �̸� ���� (��: "NewButton_1", "NewButton_2", ...)
        newButton.name = "NewButton_" + newButton.GetInstanceID();
    }

    // ���ͷ��� ��ư �ϳ� �����
    public void OnClickDeleteInteractionBtn(string btnName)
    {
        for(int i = 0; i < allSessions.sessions.Count; i++)
        {
            // ���ǵ� �� ���� �������� ���ͷ��� ��ư�鿡 ���� ���� ��� �ִ� �� ã��
            if(allSessions.sessions[i].page == pdfViewer.CurrentPageIndex)
            {
                for(int j= 0; i< allSessions.sessions[i].buttonPositions.Count; j++)
                {
                    // ���� �������� ��ư�� �� ���� ���� ��ư�� ���� ã��
                    if(allSessions.sessions[i].buttonPositions[j].buttonName == btnName)
                    {
                        // ������ �����
                        allSessions.sessions[i].buttonPositions.RemoveAt(j);
                    }
                }
            }
        }
    }

    public void SaveCurrentSession()
    {
        print("Save Current Session");
        ButtonPositionData currentSession = new ButtonPositionData();

        foreach (Transform child in teachingData.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            InteractionMakeBtn interactionBtn = child.GetComponent<InteractionMakeBtn>();

            // ���� ���� �ִ� ������ ���� 
            currentSession.page = pdfViewer.CurrentPageIndex;

            if (interactionBtn.Item == null)
            {
                continue;
            }

            currentSession.buttonPositions.Add(new ButtonPosition
            {
                buttonName = child.name,
                posX = rectTransform.anchoredPosition.x,
                posY = rectTransform.anchoredPosition.y,

                // ���õ� ������ ���� ����
                item = interactionBtn.Item
            });
        }

        //// ������ �ִ� ���� ������ ���ͷ��� ��ư�鿡 ���� ���� ����� �ٽ� �����Ұ���. 
        //for(int i = 0; i < allSessions?.sessions?.Count; i++)
        //{
        //    if(allSessions.sessions[i].page == pdfViewer.CurrentPageIndex)
        //    {
        //        allSessions.sessions.RemoveAt(i);
        //        break;
        //    }
        //}

        allSessions.sessions.Add(currentSession);
        json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
        //UpdateSessionDropdown();
    }

    public void LoadSelectedSession(int curPage)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        // ���� - ���� �߿��� RPC�� �л������׵� ���ͷ��� ��ư ������
        if(buildIndex == 4 ) 
        {
            if (isLesson)
            {
                photonView.RPC(nameof(LoadInteractionRPC), RpcTarget.All, json, curPage);
            }
            else
            {
                DestroyAllButtons();

                foreach (ButtonPositionData buttonPositionData in allSessions.sessions)
                {
                    // ������ �̵� �Լ��� ���ÿ� ȣ��Ǳ� ������ �̵� ���� �������� �Ѿ��.
                    // �˾Ƽ� ���� or ���� �������� ���
                    if (curPage == buttonPositionData.page)
                    {
                        foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                        //foreach (ButtonPosition buttonPosition in selectedSession.buttonPositions)
                        {
                            // �����߿��� �л������׵� ���ͷ��� ��ư ����
                            GameObject newButton = Instantiate(inClassButtonPrefab, teachingData.transform);
                            newButton.name = buttonPosition.buttonName;
                            RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                            rectTransform.anchoredPosition = new Vector2(buttonPosition.posX, buttonPosition.posY);
                            newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(buttonPosition.item.itemPath, true)));
                            //byte[] itemBytes = File.ReadAllBytes(buttonPosition.item.itemPath);
                            //photonView.RPC(nameof(LoadInteractionRPC), RpcTarget.All, ((int)buttonPosition.item.itemType), buttonPosition.item.itemName, itemBytes, buttonPosition.buttonName, buttonPosition.posX, buttonPosition.posY);
                        }
                    }
                }
            }
        }

        // ���� - ����X , ������ ���� ������������ �����Ը�. (RPC ������ ����)
        else 
        {
            // ������������ �ִ� ��ư�� �� ����� �ε��ϱ�
            DestroyAllButtons();

            foreach(ButtonPositionData buttonPositionData in allSessions.sessions)
            {
                // ������ �̵� �Լ��� ���ÿ� ȣ��Ǳ� ������ �̵� ���� �������� �Ѿ��.
                // �˾Ƽ� ���� or ���� �������� ���
                if(curPage == buttonPositionData.page)
                {
                    foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                    //foreach (ButtonPosition buttonPosition in selectedSession.buttonPositions)
                    {
                        GameObject newButton = Instantiate(selectItemButtonPrefab, teachingData.transform);
                        newButton.name = buttonPosition.buttonName;
                        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(buttonPosition.posX, buttonPosition.posY);
                        newButton.GetComponent<InteractionMakeBtn>().Item = buttonPosition.item;
                    }
                }
            }
        }
    }

    [PunRPC]
    // Item ������ ���� �� ��� string Ÿ������ JSON ��ü�� ��������.
    public void LoadInteractionRPC(string json, int curPage)
    {
        DestroyAllButtons();

        ButtonSessions buttonSessions = JsonUtility.FromJson<ButtonSessions>(json);

        foreach (ButtonPositionData buttonPositionData in buttonSessions.sessions)
        {
            // ������ �̵� �Լ��� ���ÿ� ȣ��Ǳ� ������ �̵� ���� �������� �Ѿ��.
            // �˾Ƽ� ���� or ���� �������� ���
            if (curPage == buttonPositionData.page)
            {
                foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                {
                    GameObject newButton = Instantiate(inClassButtonPrefab, teachingData.transform);
                    newButton.name = buttonPosition.buttonName;
                    RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(buttonPosition.posX, buttonPosition.posY);

                    // �����۵��� ���ÿ� ����־ �ٸ� ����̽����� ���� ������ ���� �������Ⱑ �ȵ�....
                    // �����۵� ���/ ���� �̸����� �Ȱ��ƾ� ��
                    newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(buttonPosition.item.itemPath, true)));

                    //newButton.GetComponent<Interaction_InClassBtn>().SetItem(buttonPosition.item);
                    //newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(newButton.GetComponent<Interaction_InClassBtn>().item));
                }
            }
        }
    }

    [PunRPC]
    // Item ������ ���� �� ��� string Ÿ������ JSON ��ü�� ��������.
    public void LoadInteractionRPC(int itemType, string itemName, byte[] itemBytes, string buttonName, float posX, float posY)
    {
        GameObject newButton = Instantiate(inClassButtonPrefab, teachingData.transform);
        newButton.name = buttonName;
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(posX, posY);

        // �����۵��� ���ÿ� ����־ �ٸ� ����̽����� ���� ������ ���� �������Ⱑ �ȵ�....
        // �����۵� ���/ ���� �̸����� �Ȱ��ƾ� ��
        newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(new Item((Item.ItemType)itemType, itemName, itemBytes)));
    }

    [PunRPC]
    public void DestroyAllButtons()
    {
        foreach (Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }
    }

    public void ShowItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Image:
                showItemRawImage.texture = item.itemTexture;
                showItemImage.preserveAspect = true;
                showItemRawImage.gameObject.SetActive(true);
                break;
            case Item.ItemType.GIF:
                gifLoad.Show(showItemImage, item.gifSprites, item.gifDelayTime);
                showItemImage.gameObject.SetActive(true);
                break;
            case Item.ItemType.Video:
                videoPlayer.url = item.itemPath;
                showItemRawImage.texture = videoPlayer.targetTexture;
                videoPlayer.Play();
                showItemRawImage.gameObject.SetActive(true);
                break;
            case Item.ItemType.Object:
                break;
            default:
                break;
        }
        showItemPanel.SetActive(true);
    }

    // �������� ������(�̹���, GIF, ����) �ݱ�
    public void CloseShowItem()
    {
        showItemPanel.SetActive(false);
        showItemImage.gameObject.SetActive(false);
        showItemRawImage.gameObject.SetActive(false);
    }

    private void LoadAllSessions()
    {
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
            allSessions = JsonUtility.FromJson<ButtonSessions>(json);
        }
        else
        {
            allSessions = new ButtonSessions();
            allSessions.sessions = new List<ButtonPositionData>();
            //print(allSessions.sessions.Count);
        }
    }

    public void DeleteSelectedSession()
    {
        //int selectedSessionIndex = sessionDropdown.value;

        //// ���õ� ������ ����Ʈ���� ����
        //if (selectedSessionIndex >= 0 && selectedSessionIndex < allSessions.sessions.Count)
        //{
        //    allSessions.sessions.RemoveAt(selectedSessionIndex);
        //}

        // ������������ �ִ� ��ư�� ���� ����
        foreach (Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }

        // ���� �����ִ� �������� ���ͷ��� ���� ����
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            if(pdfViewer.CurrentPageIndex == allSessions.sessions[i].page)
            {
                allSessions.sessions.RemoveAt(i);
            }
        }

        // ����� ���� �����͸� ����
        SaveAllSessions();

        // ��Ӵٿ� �޴� ������Ʈ
        //UpdateSessionDropdown();
    }

    private void SaveAllSessions()
    {
        json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
    }
}
