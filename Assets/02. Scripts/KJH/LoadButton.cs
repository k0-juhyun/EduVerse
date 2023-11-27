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
    public static LoadButton instance = null;

    private bool isLesson = false;        // 수업중인지 확인 (교실에서 수업중이지 않을 떄 pdf를 넘겨도 인터렉션 버튼 만들지 않고 수업이 끝나면 만들어져 있던 인터렉션 버튼들 지움)
    private string json;                  // 인터렉션 버튼 정보 json RPC로 넘겨줄거임
    private string filePath;
    private ButtonSessions allSessions;
    [HideInInspector]public ScreenTransition screenTransition;

    [Space(10)]
    public Button createBtn;
    public Button saveBtn;


    [Space (10)]
    public GameObject selectItemButtonPrefab;           // 인터렉션할 아이템 선택하는 버튼
    public GameObject inClassButtonPrefab;              // 수업시간에 아이템 사용할 버튼

    [Space(10)]
    public GameObject teachingData;

    [Space(10)]
    public GameObject interactionCanvas;
    public GameObject blurPanel;
    public GameObject showItemPanel;
    public Image showItemImage;
    public RawImage showItemRawImage;
    public GifLoad gifLoad;
    public VideoPlayer videoPlayer;
    public Button closeBtn;
    public Button lessonBtn;                          // 수업 집중모드 버튼
    public GameObject trashCan;

    [HideInInspector] public PhotonView myPhotonView;

    [Space(10)]
    public Paroxe.PdfRenderer.PDFViewer pdfViewer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

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

    public void Interaction(bool isInteraction)
    {
        interactionCanvas.SetActive(isInteraction);
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

    // 인터렉션 버튼 하나 지우기
    public void OnClickDeleteInteractionBtn(string btnName)
    {
        for(int i = 0; i < allSessions.sessions.Count; i++)
        {
            // 세션들 중 현재 페이지의 인터렉션 버튼들에 대한 정보 담고 있는 거 찾기
            if(allSessions.sessions[i].page == pdfViewer.CurrentPageIndex)
            {
                for(int j= 0; j < allSessions.sessions[i].buttonPositions.Count; j++)
                {
                    // 현재 페이지의 버튼들 중 내가 지울 버튼의 정보 찾기
                    if(allSessions.sessions[i].buttonPositions[j].buttonName == btnName)
                    {
                        // 있으면 지우기
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

            // 현재 보고 있는 페이지 저장 
            currentSession.page = pdfViewer.CurrentPageIndex;

            // 아이템 정보가 할당
            if (interactionBtn.Item == null)
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

        //// 기존에 있던 현재 페이지 인터렉션 버튼들에 대한 정보 지우고 다시 저장할거임. 
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
        print("load selected session");
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        // 교실 - 수업 중에는 RPC로 학생들한테도 인터렉션 버튼 보내기
        if(buildIndex == 4 ) 
        {
            if (interactionCanvas.activeSelf)
            {
                //photonView.RPC(nameof(LoadInteractionRPC), RpcTarget.All, json, curPage);

                photonView.RPC(nameof(DestroyAllButtons), RpcTarget.All);
                foreach (ButtonPositionData buttonPositionData in allSessions.sessions)
                {
                    if (curPage == buttonPositionData.page)
                    {
                        foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                        {
                            print(buttonPosition.buttonName);
                            photonView.RPC(nameof(LoadInteractionRPC), RpcTarget.All, (int)buttonPosition.item.itemType, buttonPosition.item.itemName, buttonPosition.item.itemPath, buttonPosition.buttonName, buttonPosition.posX, buttonPosition.posY);
                        }
                    }
                }
            }
            else
            {
                DestroyAllButtons();

                foreach (ButtonPositionData buttonPositionData in allSessions.sessions)
                {
                    // 페이지 이동 함수가 동시에 호출되기 때문에 이동 전의 페이지가 넘어옴.
                    // 알아서 다음 or 이전 페이지로 계산
                    if (curPage == buttonPositionData.page)
                    {
                        foreach (ButtonPosition buttonPosition in buttonPositionData.buttonPositions)
                        //foreach (ButtonPosition buttonPosition in selectedSession.buttonPositions)
                        {
                            // 수업중에는 학생들한테도 인터렉션 버튼 보임
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

        // 교실 - 수업X , 교과서 제작 페이지에서는 선생님만. (RPC 보내지 않음)
        else 
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
    }

    [PunRPC]
    // Item 정보를 보낼 수 없어서 string 타입으로 JSON 자체를 보내버림.
    public void LoadInteractionRPC(string json, int curPage)
    {
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

                    // 아이템들이 로컬에 들어있어서 다른 디바이스에서 보낸 아이템 정보 가져오기가 안됨....
                    // 아이템들 경로/ 파일 이름까지 똑같아야 함
#if UNITY_ANDROID
                    switch (buttonPosition.item.itemType)
                    {
                        case Item.ItemType.Image:
                            buttonPosition.item.itemPath = Application.persistentDataPath + "/MarketItems/" + Path.GetFileName(buttonPosition.item.itemPath);
                            break;
                        case Item.ItemType.GIF:
                            buttonPosition.item.itemPath = Application.persistentDataPath + "/GIF/" + Path.GetFileName(buttonPosition.item.itemPath);
                            break;
                        case Item.ItemType.Video:
                            buttonPosition.item.itemPath = Application.persistentDataPath + "/Videos/" + Path.GetFileName(buttonPosition.item.itemPath);
                            break;
                    }
#endif

                    print(buttonPosition.item.itemPath);
                    newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(buttonPosition.item.itemPath, true)));

                    //newButton.GetComponent<Interaction_InClassBtn>().SetItem(buttonPosition.item);
                    //newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(newButton.GetComponent<Interaction_InClassBtn>().item));
                }
            }
        }
    }

    [PunRPC]
    // Item 정보를 보낼 수 없어서 string 타입으로 JSON 자체를 보내버림.
    public void LoadInteractionRPC(int itemType, string itemName, string itemPath, string buttonName, float posX, float posY)
    {
        GameObject newButton = Instantiate(inClassButtonPrefab, teachingData.transform);
        newButton.name = buttonName;
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(posX, DataBase.instance.userInfo.isteacher ? posY : posY * (800f / 642f));

        // 아이템들이 로컬에 들어있어서 다른 디바이스에서 보낸 아이템 정보 가져오기가 안됨....
        // 아이템들 경로/ 파일 이름까지 똑같아야 함
        //newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(new Item((Item.ItemType)itemType, itemName, itemPath)));
#if UNITY_EDITOR
#elif UNITY_ANDROID
        switch ((Item.ItemType)itemType) 
        {
            case Item.ItemType.Image:
                itemPath = Application.persistentDataPath + "/MarketItems/" + Path.GetFileName(itemPath);
                break;
            case Item.ItemType.GIF:
                itemPath = Application.persistentDataPath + "/GIF/" + Path.GetFileName(itemPath);
                break;
            case Item.ItemType.Video:
                itemPath = Application.persistentDataPath + "/Videos/" + Path.GetFileName(itemPath);
                break;
        }
#endif

        print(itemPath);
        Item item = MyItemsManager.instance.GetItemInfo(itemPath, true);
        print(item.itemPath);
        newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(itemPath, true)));
        //newButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(MyItemsManager.instance.GetItemInfo(itemPath, true)));
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
        print(item == null);
        print(item?.itemName);

        Vector2 sizeDelta = Vector2.zero;

        switch (item.itemType)
        {
            case Item.ItemType.Image:
                showItemRawImage.texture = item.itemTexture;
                showItemImage.preserveAspect = true;
                showItemRawImage.gameObject.SetActive(true);
                sizeDelta = new Vector2(item.itemTexture.width + 30f, item.itemTexture.height + 40f);
                break;
            case Item.ItemType.GIF:
                print("switch");
                gifLoad.Show(showItemImage, item.gifSprites, item.gifDelayTime);
                showItemImage.gameObject.SetActive(true);
                showItemImage.preserveAspect = true;
                sizeDelta = new Vector2(700f, item.gifSprites[0].texture.height + 40f);
                break;
            case Item.ItemType.Video:
                videoPlayer.url = item.itemPath;
                showItemRawImage.texture = videoPlayer.targetTexture;
                videoPlayer.Play();
                showItemRawImage.gameObject.SetActive(true);
                sizeDelta = new Vector2(700f, 450f);
                break;
            case Item.ItemType.Object:
                break;
            default:
                break;
        }

        blurPanel.SetActive(true);
        showItemPanel.SetActive(true);
        showItemPanel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

    // 실행중인 아이템(이미지, GIF, 영상) 닫기
    public void CloseShowItem()
    {
        print("close");
        blurPanel.SetActive(false);
        showItemPanel.SetActive(false);
        showItemImage.gameObject.SetActive(false);
        showItemRawImage.gameObject.SetActive(false);
    }

    private void LoadAllSessions()
    {
        print("Load All Sessions");
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
    }
}
