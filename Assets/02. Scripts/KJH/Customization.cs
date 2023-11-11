using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CustomPart
{
    public string partName; // 커스텀할 파츠 이름
    public GameObject partObj; // 커스텀할 파츠 오브젝트
    public string objName;
    public Mesh[] partList; // 교체 메쉬들
    public Image[] partImages;
    public Sprite[] partSprites; // 스프라이트들
    public Button[] partButton; // 메쉬 선택을 위한 버튼들
    [HideInInspector]
    public int currentIdx; // 현재 인덱스
    public SkinnedMeshRenderer customRenderer; // 메쉬 렌더러
}

public class Customization : MonoBehaviour
{
    [Header("목록")]
    public CustomPart[] customParts;

    private GameObject lastClickedButtonParent;

    [Space(10)]
    [Header("버튼")]
    public Button 헤어버튼;
    public Button 눈버튼;
    public Button 입버튼;
    public Button 의상버튼;
    public Button 바지버튼;
    public Button 신발버튼;

    [Space(10)]
    [Header("스크롤뷰")]
    public GameObject 헤어스크롤뷰;
    public GameObject 눈스크롤뷰;
    public GameObject 입스크롤뷰;
    public GameObject 옷스크롤뷰;
    public GameObject 바지스크롤뷰;
    public GameObject 신발스크롤뷰;

    public TMP_Text 현재커스텀부위;

    private void Awake()
    {
        // 컴포넌트 할당 및 초기화
        foreach (var part in customParts)
        {
            part.objName = part.partObj.gameObject.name;
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;

            // 버튼 이벤트 할당
            //for (int i = 0; i < part.partButton?.Length; i++)
            //{
            //    int meshIndex = i; // 메쉬 인덱스
            //    part.partButton[i]?.onClick.AddListener(() => SetMesh(part, meshIndex));
            //}

            foreach (var button in part.partButton)
            {
                if (button != null)
                {
                    // Outline 컴포넌트 추가 및 초기화
                    GameObject parentObject = button.transform.parent.gameObject;
                    Outline outline = parentObject.GetComponent<Outline>();
                    if (outline == null)
                    {
                        outline = parentObject.AddComponent<Outline>();
                    }
                    outline.enabled = false; // 초기에 비활성화

                    // 버튼 이벤트 할당
                    int meshIndex = System.Array.IndexOf(part.partButton, button);
                    button.onClick.AddListener(() => SetMesh(part, meshIndex));
                    button.onClick.AddListener(() => ToggleButtonParentOutline(parentObject));
                }
            }

            // 이미지 초기화
            for (int j = 0; j < part.partImages?.Length; j++)
            {
                part.partImages[j].sprite = part.partSprites[j];
            }
        }

        // 데이터베이스 초기화
        for (int i = 0; i < customParts.Length; i++)
        {
            DataBase.instance.myInfo.meshObjName.Add(customParts[i].objName);
            DataBase.instance.myInfo.meshIndex.Add(customParts[i].currentIdx);
        }
    }

    private void Start()
    {
        // 버튼 리스너 설정
        헤어버튼.onClick.AddListener(() => ToggleScrollViews(헤어스크롤뷰, 헤어버튼));
        눈버튼.onClick.AddListener(() => ToggleScrollViews(눈스크롤뷰, 눈버튼));
        입버튼.onClick.AddListener(() => ToggleScrollViews(입스크롤뷰, 입버튼));
        의상버튼.onClick.AddListener(() => ToggleScrollViews(옷스크롤뷰, 의상버튼));
        바지버튼.onClick.AddListener(() => ToggleScrollViews(바지스크롤뷰, 바지버튼));
        신발버튼.onClick.AddListener(() => ToggleScrollViews(신발스크롤뷰, 신발버튼));
    }

    private void ToggleScrollViews(GameObject activeScrollView, Button activeButton)
    {
        // 모든 스크롤 뷰 비활성화
        헤어스크롤뷰.SetActive(false);
        눈스크롤뷰.SetActive(false);
        입스크롤뷰.SetActive(false);
        옷스크롤뷰.SetActive(false);
        바지스크롤뷰.SetActive(false);
        신발스크롤뷰.SetActive(false);

        // 선택된 스크롤 뷰 활성화
        activeScrollView.SetActive(true);

        print(activeScrollView);
        // 현재 커스텀 부위 텍스트 업데이트
        UpdateCurrentCustomPartText(activeButton);
    }

    private void ToggleButtonParentOutline(GameObject clickedButtonParent)
    {
        if (lastClickedButtonParent != null)
        {
            // 이전에 클릭된 버튼의 부모 Outline 비활성화
            Outline lastOutline = lastClickedButtonParent.GetComponent<Outline>();
            if (lastOutline != null)
            {
                lastOutline.enabled = false;
            }
        }

        // 현재 클릭된 버튼의 부모 Outline 활성화
        Outline currentOutline = clickedButtonParent.GetComponent<Outline>();
        if (currentOutline != null)
        {
            currentOutline.enabled = true;
            lastClickedButtonParent = clickedButtonParent; // 현재 버튼의 부모를 마지막으로 클릭된 것으로 저장
        }
    }

    private void UpdateCurrentCustomPartText(Button activeButton)
    {
        TMP_Text buttonText = activeButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            현재커스텀부위.text = buttonText.text;
        }
    }

    // 메쉬 설정
    private void SetMesh(CustomPart part, int meshIndex)
    {
        if (part.customRenderer != null && meshIndex >= 0 && meshIndex < part.partList.Length)
        {
            part.customRenderer.sharedMesh = part.partList[meshIndex];
            part.currentIdx = meshIndex;
            // 데이터베이스 업데이트
            int index = System.Array.IndexOf(customParts, part);
            DataBase.instance.myInfo.meshIndex[index] = meshIndex;
        }
    }

}
