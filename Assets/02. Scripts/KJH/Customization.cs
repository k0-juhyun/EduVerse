using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CustomPart
{
    public string partName; // Ŀ������ ���� �̸�
    public GameObject partObj; // Ŀ������ ���� ������Ʈ
    public string objName;
    public Mesh[] partList; // ��ü �޽���
    public Image[] partImages;
    public Sprite[] partSprites; // ��������Ʈ��
    public Button[] partButton; // �޽� ������ ���� ��ư��
    [HideInInspector]
    public int currentIdx; // ���� �ε���
    public SkinnedMeshRenderer customRenderer; // �޽� ������
}

public class Customization : MonoBehaviour
{
    [Header("���")]
    public CustomPart[] customParts;

    private GameObject lastClickedButtonParent;

    private bool isScrolling = false;

    public GameObject clothGameObject;
    public GameObject colorGameObject;

    [Space(10)]
    [Header("��ư")]
    public Button hairButton;
    public Button eyeButton;
    public Button mouthButton;
    public Button clothButton;
    public Button pantButton;
    public Button shoeButton;
    private GameObject lastActiveButtonParent;

    [Space(10)]
    [Header("��ư ȿ����")]
    public AudioClip buttonSfx;

    [Space(10)]
    [Header("��ũ�Ѻ�")]
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

    public TMP_Text ����Ŀ���Һ���;

    private void Awake()
    {
        // ������Ʈ �Ҵ� �� �ʱ�ȭ
        foreach (var part in customParts)
        {
            part.objName = part.partObj.gameObject.name;
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;

            // ��ư �̺�Ʈ �Ҵ�
            //for (int i = 0; i < part.partButton?.Length; i++)
            //{
            //    int meshIndex = i; // �޽� �ε���
            //    part.partButton[i]?.onClick.AddListener(() => SetMesh(part, meshIndex));
            //}

            foreach (var button in part.partButton)
            {
                if (button != null)
                {
                    // Outline ������Ʈ �߰� �� �ʱ�ȭ
                    GameObject parentObject = button.transform.parent.gameObject;
                    GameObject parentParentObject = parentObject.transform.parent.gameObject;

                    // ��ư �̺�Ʈ �Ҵ�
                    int meshIndex = System.Array.IndexOf(part.partButton, button);
                    button.onClick.AddListener(() => SetMesh(part, meshIndex));
                    button.onClick.AddListener(() => ToggleButtonParentOutline(parentParentObject));
                    button.onClick.AddListener(() => SoundManager.instance.PlaySFX(SoundManager.SFXClip.Button1));
                    //button.gameObject.AddComponent<ButtonClickSoundHandler>().ButtonClickSound = buttonSfx;
                }
            }

            // �̹��� �ʱ�ȭ
            for (int j = 0; j < part.partImages?.Length; j++)
            {
                part.partImages[j].sprite = part.partSprites[j];
            }
        }

        // �����ͺ��̽� �ʱ�ȭ
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
        // ��ư ������ ����
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

        // �ٸ� ��� ��ũ�� ���� ���¸� false�� ����
        FindActivateScrollView(false);

        // ���õ� ��ũ�� ���� ���¸� true�� ����
        activeState = true;

        // ��� ��ũ�� �� ��Ȱ��ȭ
        hairScrollView.SetActive(false);
        eyeScrollView.SetActive(false);
        mouthScrollView.SetActive(false);
        clothScrollView.SetActive(false);
        pantsScrollView.SetActive(false);
        shoesScrollView.SetActive(false);

        // ���õ� ��ũ�� �� Ȱ��ȭ
        activeScrollView.SetActive(true);

        print(activeScrollView);
        // ���� Ŀ���� ���� �ؽ�Ʈ ������Ʈ
        UpdateCurrentCustomPartText(activeButton);

        ChangeButtonParentColor(activeButton.gameObject);
    }

    private void ChangeButtonParentColor(GameObject currentButtonParent)
    {
        if (lastActiveButtonParent != null)
        {
            // ���� ��ư �θ� ������Ʈ ������ ������� ����
            Image lastParentImage = lastActiveButtonParent.GetComponent<Image>();
            if (lastParentImage != null)
            {
                lastParentImage.color = Color.white; // ���� �������� ����
            }
        }

        // ���� ��ư �θ� ������Ʈ ������ ���������� ����
        Image currentParentImage = currentButtonParent.GetComponent<Image>();
        if (currentParentImage != null)
        {
            currentParentImage.color = Color.red;
        }

        // ���������� Ȱ��ȭ�� ��ư �θ� ������Ʈ ������Ʈ
        lastActiveButtonParent = currentButtonParent;
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
        // ���� Ŭ���� ��ư
        Image currentOutline = clickedButtonParent.GetComponent<Image>();
        Color originColor = currentOutline.color;

        if (lastClickedButtonParent != null)
        {
            // ������ Ŭ���� ��ư ���󺯰�
            Image lastOutline = lastClickedButtonParent.GetComponent<Image>();
            if (lastOutline != null)
            {
                lastOutline.color = originColor;
            }
        }

        if (currentOutline != null)
        {
            currentOutline.color = Color.red;
            // ���� ��ư�� �θ� ���������� Ŭ���� ������ ����
            lastClickedButtonParent = clickedButtonParent; 
        }
    }

    private void UpdateCurrentCustomPartText(Button activeButton)
    {
        TMP_Text buttonText = activeButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            ����Ŀ���Һ���.text = buttonText.text;
        }
    }

    // �޽� ����
    private void SetMesh(CustomPart part, int meshIndex)
    {
        if (part.customRenderer != null && meshIndex >= 0 && meshIndex < part.partList.Length)
        {
            part.customRenderer.sharedMesh = part.partList[meshIndex];
            part.currentIdx = meshIndex;
            // �����ͺ��̽� ������Ʈ
            int index = System.Array.IndexOf(customParts, part);
            DataBase.instance.myInfo.meshIndex[index] = meshIndex;
        }
    }

    private void SetColor(CustomPart part, Color color)
    {
        if (part.customRenderer != null)
        {
            Material newMaterial = new Material(Shader.Find("Standard")); // ������ ���̴� ���
            newMaterial.color = color;
            Material[] materials = part.customRenderer.materials;
            materials[0] = newMaterial; // ���÷� ù ��° Material�� ��ü
            part.customRenderer.materials = materials;
        }
    }

    public void ChangeColorBasedOnActiveScrollView(Button selectedButton)
    {
        Color selectedColor = selectedButton.GetComponent<Image>().color;

        if (hairScrollView.activeSelf)
        {
            CustomPart hairPart = Array.Find(customParts, part => part.partName == "���");
            SetColor(hairPart, selectedColor);
        }
        else if (eyeScrollView.activeSelf)
        {
            CustomPart eyePart = Array.Find(customParts, part => part.partName == "��");
            SetColor(eyePart, selectedColor);
        }
        else if (mouthScrollView.activeSelf)
        {
            CustomPart mouthPart = Array.Find(customParts, part => part.partName == "��");
            SetColor(mouthPart, selectedColor);
        }
        else if (clothScrollView.activeSelf)
        {
            CustomPart clothPart = Array.Find(customParts, part => part.partName == "����");
            SetColor(clothPart, selectedColor);
        }
        else if (mouthScrollView.activeSelf)
        {
            CustomPart pantPart = Array.Find(customParts, part => part.partName == "����");
            SetColor(pantPart, selectedColor);
        }
        else if (shoesScrollView.activeSelf)
        {
            CustomPart shoePart = Array.Find(customParts, part => part.partName == "�Ź�");
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
