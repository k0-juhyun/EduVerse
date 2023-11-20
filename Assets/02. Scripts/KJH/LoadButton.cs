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
    private string json;                  // 인터렉션 버튼 정보 json RPC로 넘겨줄거임
    private string filePath;
    private ButtonSessions allSessions;

    [Space(10)]
    public Button endCLassBtn;
    public Button createBtn;
    public Button saveBtn;


    [Space (10)]
    public GameObject selectItemButtonPrefab;           // 인터렉션할 아이템 선택하는 버튼
    public GameObject inClassButtonPrefab;              // 수업시간에 아이템 사용할 버튼

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

                // 현재 보고 있는 페이지 저장 
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

                    // 선택된 아이템 정보 저장
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

        // 교과서 제작페이지
        if(buildIndex == 2)
        {
            // 이전페이지에 있던 버튼들 싹 지우고 로드하기
            DestroyAllButtons();

            foreach(ButtonPositionData buttonPositionData in allSessions.sessions)
            {
                // 페이지 이동 함수가 동시에 호출되기 때문에 이동 전의 페이지가 넘어옴.
                // 알아서 다음 or 이전 페이지로 계산
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

        // 교실(수업)
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
            // 페이지 이동 함수가 동시에 호출되기 때문에 이동 전의 페이지가 넘어옴.
            // 알아서 다음 or 이전 페이지로 계산
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

    // 실행중인 아이템(이미지, GIF, 영상) 닫기
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
        json = JsonUtility.ToJson(allSessions);
        File.WriteAllText(filePath, json);

        print("save all sessions : " + (allSessions == null).ToString() + ", " + (allSessions.sessions == null).ToString() + ", " + (allSessions.sessions == null ? "" : allSessions.sessions.Count));
    }
}
