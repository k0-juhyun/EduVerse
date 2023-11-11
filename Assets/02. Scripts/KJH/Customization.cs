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

    [Space(10)]
    [Header("��ư")]
    public Button ����ư;
    public Button ����ư;
    public Button �Թ�ư;
    public Button �ǻ��ư;
    public Button ������ư;
    public Button �Ź߹�ư;

    [Space(10)]
    [Header("��ũ�Ѻ�")]
    public GameObject ��ũ�Ѻ�;
    public GameObject ����ũ�Ѻ�;
    public GameObject �Խ�ũ�Ѻ�;
    public GameObject �ʽ�ũ�Ѻ�;
    public GameObject ������ũ�Ѻ�;
    public GameObject �Ź߽�ũ�Ѻ�;

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
                    Outline outline = parentObject.GetComponent<Outline>();
                    if (outline == null)
                    {
                        outline = parentObject.AddComponent<Outline>();
                    }
                    outline.enabled = false; // �ʱ⿡ ��Ȱ��ȭ

                    // ��ư �̺�Ʈ �Ҵ�
                    int meshIndex = System.Array.IndexOf(part.partButton, button);
                    button.onClick.AddListener(() => SetMesh(part, meshIndex));
                    button.onClick.AddListener(() => ToggleButtonParentOutline(parentObject));
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
    }

    private void Start()
    {
        // ��ư ������ ����
        ����ư.onClick.AddListener(() => ToggleScrollViews(��ũ�Ѻ�, ����ư));
        ����ư.onClick.AddListener(() => ToggleScrollViews(����ũ�Ѻ�, ����ư));
        �Թ�ư.onClick.AddListener(() => ToggleScrollViews(�Խ�ũ�Ѻ�, �Թ�ư));
        �ǻ��ư.onClick.AddListener(() => ToggleScrollViews(�ʽ�ũ�Ѻ�, �ǻ��ư));
        ������ư.onClick.AddListener(() => ToggleScrollViews(������ũ�Ѻ�, ������ư));
        �Ź߹�ư.onClick.AddListener(() => ToggleScrollViews(�Ź߽�ũ�Ѻ�, �Ź߹�ư));
    }

    private void ToggleScrollViews(GameObject activeScrollView, Button activeButton)
    {
        // ��� ��ũ�� �� ��Ȱ��ȭ
        ��ũ�Ѻ�.SetActive(false);
        ����ũ�Ѻ�.SetActive(false);
        �Խ�ũ�Ѻ�.SetActive(false);
        �ʽ�ũ�Ѻ�.SetActive(false);
        ������ũ�Ѻ�.SetActive(false);
        �Ź߽�ũ�Ѻ�.SetActive(false);

        // ���õ� ��ũ�� �� Ȱ��ȭ
        activeScrollView.SetActive(true);

        print(activeScrollView);
        // ���� Ŀ���� ���� �ؽ�Ʈ ������Ʈ
        UpdateCurrentCustomPartText(activeButton);
    }

    private void ToggleButtonParentOutline(GameObject clickedButtonParent)
    {
        if (lastClickedButtonParent != null)
        {
            // ������ Ŭ���� ��ư�� �θ� Outline ��Ȱ��ȭ
            Outline lastOutline = lastClickedButtonParent.GetComponent<Outline>();
            if (lastOutline != null)
            {
                lastOutline.enabled = false;
            }
        }

        // ���� Ŭ���� ��ư�� �θ� Outline Ȱ��ȭ
        Outline currentOutline = clickedButtonParent.GetComponent<Outline>();
        if (currentOutline != null)
        {
            currentOutline.enabled = true;
            lastClickedButtonParent = clickedButtonParent; // ���� ��ư�� �θ� ���������� Ŭ���� ������ ����
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

}
