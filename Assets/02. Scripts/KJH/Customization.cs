using System;
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

    private bool isScrolling = false;

    public GameObject clothGameObject;
    public GameObject colorGameObject;

    [Space(10)]
    [Header("버튼")]
    public Button hairButton;
    public Button eyeButton;
    public Button mouthButton;
    public Button clothButton;
    public Button pantButton;
    public Button shoeButton;

    [Space(10)]
    [Header("스크롤뷰")]
    public GameObject hairScrollView;
    public GameObject eyeScrollView;
    public GameObject mouthScrollView;
    public GameObject clothScrollView;
    public GameObject pantsScrollView;
    public GameObject shoesScrollView;

    private RectTransform hairScrollViewRectTrnasform;
    private RectTransform eyeScrollViewRectTrnasform;
    private RectTransform mouthScrollViewRectTrnasform;
    private RectTransform clothScrollViewRectTrnasform;
    private RectTransform pantsScrollViewRectTrnasform;
    private RectTransform shoeScrollViewRectTrnasform;

    public bool isHair;
    public bool isEye;
    public bool isMouth;
    public bool isCloth;
    public bool isPant;
    public bool isShoe;

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

        hairScrollViewRectTrnasform = hairScrollView.GetComponent<RectTransform>();
        eyeScrollViewRectTrnasform = eyeScrollView.GetComponent<RectTransform>();
        mouthScrollViewRectTrnasform = mouthScrollView.GetComponent<RectTransform>();
        clothScrollViewRectTrnasform = clothScrollView.GetComponent<RectTransform>();
        pantsScrollViewRectTrnasform = pantsScrollView.GetComponent<RectTransform>();
        shoeScrollViewRectTrnasform = shoesScrollView.GetComponent<RectTransform>();
    }

    private void Start()
    {
        // 버튼 리스너 설정
        hairButton.onClick.AddListener(() => ToggleScrollViews(hairScrollView, hairButton, ref isHair));
        eyeButton.onClick.AddListener(() => ToggleScrollViews(eyeScrollView, eyeButton, ref isEye));
        mouthButton.onClick.AddListener(() => ToggleScrollViews(mouthScrollView, mouthButton, ref isMouth));
        clothButton.onClick.AddListener(() => ToggleScrollViews(clothScrollView, clothButton, ref isCloth));
        pantButton.onClick.AddListener(() => ToggleScrollViews(pantsScrollView, pantButton, ref isPant));
        shoeButton.onClick.AddListener(() => ToggleScrollViews(shoesScrollView, shoeButton, ref isShoe));
    }

    private void ToggleScrollViews(GameObject activeScrollView, Button activeButton, ref bool activeState)
    {
        clothGameObject.SetActive(true);
        colorGameObject.SetActive(false);

        // 다른 모든 스크롤 뷰의 상태를 false로 설정
        FindActivateScrollView(false);

        // 선택된 스크롤 뷰의 상태를 true로 설정
        activeState = true;

        // 모든 스크롤 뷰 비활성화
        hairScrollView.SetActive(false);
        eyeScrollView.SetActive(false);
        mouthScrollView.SetActive(false);
        clothScrollView.SetActive(false);
        pantsScrollView.SetActive(false);
        shoesScrollView.SetActive(false);

        // 선택된 스크롤 뷰 활성화
        activeScrollView.SetActive(true);

        print(activeScrollView);
        // 현재 커스텀 부위 텍스트 업데이트
        UpdateCurrentCustomPartText(activeButton);
    }

    private void FindActivateScrollView(bool isActive)
    {
        isHair = isActive;
        isEye = isActive;
        isMouth = isActive;
        isCloth = isActive;
        isPant = isActive;
        isShoe = isActive;
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

    private void SetColor(CustomPart part, Color color)
    {
        if (part.customRenderer != null)
        {
            Material newMaterial = new Material(Shader.Find("Standard")); // 적절한 셰이더 사용
            newMaterial.color = color;
            Material[] materials = part.customRenderer.materials;
            materials[0] = newMaterial; // 예시로 첫 번째 Material을 교체
            part.customRenderer.materials = materials;
        }
    }

    public void ChangeColorBasedOnActiveScrollView(Button selectedButton)
    {
        Color selectedColor = selectedButton.GetComponent<Image>().color;

        if (hairScrollView.activeSelf)
        {
            CustomPart hairPart = Array.Find(customParts, part => part.partName == "헤어");
            SetColor(hairPart, selectedColor);
        }
        else if (eyeScrollView.activeSelf)
        {
            CustomPart eyePart = Array.Find(customParts, part => part.partName == "눈");
            SetColor(eyePart, selectedColor);
        }
        else if (mouthScrollView.activeSelf)
        {
            CustomPart mouthPart = Array.Find(customParts, part => part.partName == "입");
            SetColor(mouthPart, selectedColor);
        }
        else if (clothScrollView.activeSelf)
        {
            CustomPart clothPart = Array.Find(customParts, part => part.partName == "상의");
            SetColor(clothPart, selectedColor);
        }
        else if (mouthScrollView.activeSelf)
        {
            CustomPart pantPart = Array.Find(customParts, part => part.partName == "바지");
            SetColor(pantPart, selectedColor);
        }
        else if (shoesScrollView.activeSelf)
        {
            CustomPart shoePart = Array.Find(customParts, part => part.partName == "신발");
            SetColor(shoePart, selectedColor);
        }
    }

    public void OnClickClothButton()
    {
        clothGameObject.SetActive(true);
        colorGameObject.SetActive(false);
    }

    public void OnClickColorButton()
    {
        clothGameObject.SetActive(false);
        colorGameObject.SetActive(true);
    }
}
