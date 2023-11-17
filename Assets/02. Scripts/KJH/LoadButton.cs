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

    public GameObject selectItemButtonPrefab;           // 인터렉션할 아이템 선택하는 버튼
    public GameObject inClassButtonPrefab;              // 수업시간에 아이템 사용할 버튼

    [Space(10)]
    public GameObject teachingData;
    public Dropdown sessionDropdown; // 세션을 선택하는 드롭다운

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
        //// 누른 버튼의 이름이 "Button"이 아니라면 새 버튼 생성
        //if (clickedButton.name == "Create")
        //{
        //    GameObject newButton = Instantiate(buttonPrefab, teachingData.transform);
        //    newButton.AddComponent<DraggableButton>();

        //    // 새 버튼에 이름 설정 (예: "NewButton_1", "NewButton_2", ...)
        //    newButton.name = "NewButton_" + newButton.GetInstanceID();
        //}

        // 로드된 PDF파일이 없으면 리턴.
        if (!pdfViewer.IsLoaded)
            return;

        GameObject newButton = Instantiate(selectItemButtonPrefab, teachingData.transform);

        // 새 버튼에 이름 설정 (예: "NewButton_1", "NewButton_2", ...)
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

                // 현재 보고 있는 페이지 저장 
                currentSession.page = pdfViewer.CurrentPageIndex;

                currentSession.buttonPositions.Add(new ButtonPosition
                {
                    buttonName = child.name,
                    posX = rectTransform.anchoredPosition.x,
                    posY = rectTransform.anchoredPosition.y,

                    // 선택된 아이템 정보 저장
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
        // 이전페이지에 있던 버튼들 싹 지우고 로드하기
        foreach(Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }

        //int selectedSessionIndex = sessionDropdown.value;
        //if (selectedSessionIndex < allSessions.sessions.Count)
        foreach(ButtonPositionData buttonPositionData in allSessions.sessions)
        {
            //ButtonPositionData selectedSession = allSessions.sessions[selectedSessionIndex];

            // 페이지 이동 함수가 동시에 호출되기 때문에 이동 전의 페이지가 넘어옴.
            // 알아서 다음 or 이전 페이지로 계산
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

                    // 저장된 아이템 정보 가져와서 넣기
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

    // 실행중인 아이템(이미지, GIF, 영상) 닫기
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
        // 현재 선택된 인덱스를 저장
        int selectedIndex = sessionDropdown.value;

        sessionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            options.Add("Session " + (i + 1));
        }
        sessionDropdown.AddOptions(options);

        // 드롭다운 옵션을 업데이트 한 후 이전에 선택된 인덱스를 복원
        // 저장된 세션의 수가 변경되지 않았다면 이전 인덱스를 다시 설정
        if (selectedIndex < allSessions.sessions.Count)
        {
            sessionDropdown.value = selectedIndex;
        }
        else
        {
            // 새 세션을 추가한 경우, 가장 최근 세션을 선택
            sessionDropdown.value = allSessions.sessions.Count - 1;
        }

        // 드롭다운을 강제로 업데이트
        //sessionDropdown.RefreshShownValue();
    }

    public void DeleteSelectedSession()
    {
        //int selectedSessionIndex = sessionDropdown.value;

        //// 선택된 세션을 리스트에서 제거
        //if (selectedSessionIndex >= 0 && selectedSessionIndex < allSessions.sessions.Count)
        //{
        //    allSessions.sessions.RemoveAt(selectedSessionIndex);
        //}

        // 현재페이지에 있는 버튼들 전부 삭제
        foreach (Transform tr in teachingData.transform)
        {
            Destroy(tr.gameObject);
        }

        // 현재 펼쳐있는 페이지의 인터렉션 정보 삭제
        for (int i = 0; i < allSessions.sessions.Count; i++)
        {
            if(pdfViewer.CurrentPageIndex == allSessions.sessions[i].page)
            {
                allSessions.sessions.RemoveAt(i);
            }
        }

        // 변경된 세션 데이터를 저장
        SaveAllSessions();

        // 드롭다운 메뉴 업데이트
        //UpdateSessionDropdown();
    }

    private void SaveAllSessions()
    {
        string json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);
    }
}
