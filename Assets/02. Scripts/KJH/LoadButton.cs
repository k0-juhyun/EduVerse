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
    private string json;                  // ���ͷ��� ��ư ���� json RPC�� �Ѱ��ٰ���
    private string filePath;
    private ButtonSessions allSessions;

    [Space(10)]
    public Button endCLassBtn;
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

    [Space(10)]
    public Paroxe.PdfRenderer.PDFViewer pdfViewer;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "buttonSessions.json");
        LoadAllSessions();
    }

    private void Start()
    {
        closeBtn?.onClick.AddListener(CloseShowItem);
        endCLassBtn?.onClick.AddListener(() => photonView.RPC( nameof(DestroyAllButtonsRPC), RpcTarget.All));
        createBtn?.onClick.AddListener(OnClickCreateButton);
        saveBtn.onClick.AddListener(SaveCurrentSession);
    }
    public void OnClickCreateButton()
    {
        print("on click create button 1 : " + (allSessions == null).ToString() );
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

        print("on click create button 2 : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
    }

    public void SaveCurrentSession()
    {
        ButtonPositionData currentSession = new ButtonPositionData();

        foreach (Transform child in teachingData.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                InteractionMakeBtn interactionBtn = child.GetComponent<InteractionMakeBtn>();

                // ���� ���� �ִ� ������ ���� 
                currentSession.page = pdfViewer.CurrentPageIndex;

                if(interactionBtn.Item == null)
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
        }

        print(allSessions == null);
        print(allSessions.sessions.Count);
        print(currentSession == null);

        allSessions.sessions.Add(currentSession);
        json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
        //UpdateSessionDropdown();

        print("save current session : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
    }

    public void LoadSelectedSession(int curPage)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        // ������ ����������
        if(buildIndex == 2)
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

        // ����(����)
        else
        {
            photonView.RPC(nameof(LoadInteractionRPC), RpcTarget.All, json, curPage);
        }

        print("load selected session : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
    }

    [PunRPC]
    public void LoadInteractionRPC(string json, int curPage)
    {
        print("rpcrpcrpc");

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

                    newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(buttonPosition.item.itemPath)));
                }
            }
        }
    }

    public void DestroyAllButtons()
    {
        foreach (Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }
    }

    [PunRPC]
    public void DestroyAllButtonsRPC()
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
        print("load all session");

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

        print("load all sessions : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
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

        print("save all sessions : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
    }
}
