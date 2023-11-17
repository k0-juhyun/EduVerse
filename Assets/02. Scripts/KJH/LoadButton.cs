using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

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

public class LoadButton : MonoBehaviour
{
    private string filePath;
    private ButtonSessions allSessions;

    public GameObject selectItemButtonPrefab;           // ���ͷ����� ������ �����ϴ� ��ư
    public GameObject inClassButtonPrefab;              // �����ð��� ������ ����� ��ư

    [Space(10)]
    public GameObject teachingData;
    public Dropdown sessionDropdown; // ������ �����ϴ� ��Ӵٿ�

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
        //UpdateSessionDropdown();
    }

    private void Start()
    {
        closeBtn?.onClick.AddListener(CloseShowItem);
    }

    public void OnClickCreateButton(Button clickedButton)
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

        allSessions.sessions.Add(currentSession);
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
        //UpdateSessionDropdown();
    }

    public void LoadSelectedSession(bool isNext)
    {
        print("load");
        print( isNext ? pdfViewer.CurrentPageIndex + 2 : pdfViewer.CurrentPageIndex - 2);
        // ������������ �ִ� ��ư�� �� ����� �ε��ϱ�
        foreach(Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }

        //int selectedSessionIndex = sessionDropdown.value;
        //if (selectedSessionIndex < allSessions.sessions.Count)
        foreach(ButtonPositionData buttonPositionData in allSessions.sessions)
        {
            //ButtonPositionData selectedSession = allSessions.sessions[selectedSessionIndex];

            // ������ �̵� �Լ��� ���ÿ� ȣ��Ǳ� ������ �̵� ���� �������� �Ѿ��.
            // �˾Ƽ� ���� or ���� �������� ���
            if((isNext ? pdfViewer.CurrentPageIndex + 2 : pdfViewer.CurrentPageIndex - 2) == buttonPositionData.page)
            {
                foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                //foreach (ButtonPosition buttonPosition in selectedSession.buttonPositions)
                {
                    GameObject newButton = Instantiate(inClassButtonPrefab, teachingData.transform);
                    newButton.name = buttonPosition.buttonName;
                    RectTransform rectTransform = newButton.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(buttonPosition.posX, buttonPosition.posY);
                    //newButton.AddComponent<DraggableButton>();

                    // ����� ������ ���� �����ͼ� �ֱ�
                    //newButton.GetComponent<InteractionMakeBtn>().Item = buttonPosition.item;
                    newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(buttonPosition.item.itemPath)));
                }
            }
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
                gifLoad.Show(showItemImage, item.gifSprites);
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
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            allSessions = JsonUtility.FromJson<ButtonSessions>(json);
        }
        else
        {
            allSessions = new ButtonSessions();
        }
    }

    private void UpdateSessionDropdown()
    {
        // ���� ���õ� �ε����� ����
        int selectedIndex = sessionDropdown.value;

        sessionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            options.Add("Session " + (i + 1));
        }
        sessionDropdown.AddOptions(options);

        // ��Ӵٿ� �ɼ��� ������Ʈ �� �� ������ ���õ� �ε����� ����
        // ����� ������ ���� ������� �ʾҴٸ� ���� �ε����� �ٽ� ����
        if (selectedIndex < allSessions.sessions.Count)
        {
            sessionDropdown.value = selectedIndex;
        }
        else
        {
            // �� ������ �߰��� ���, ���� �ֱ� ������ ����
            sessionDropdown.value = allSessions.sessions.Count - 1;
        }

        // ��Ӵٿ��� ������ ������Ʈ
        //sessionDropdown.RefreshShownValue();
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
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
    }
}
