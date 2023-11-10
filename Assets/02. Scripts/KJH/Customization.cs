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

    private void Awake()
    {
        // ������Ʈ �Ҵ� �� �ʱ�ȭ
        foreach (var part in customParts)
        {
            part.objName = part.partObj.gameObject.name;
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;

            // ��ư �̺�Ʈ �Ҵ�
            for (int i = 0; i < part.partButton?.Length; i++)
            {
                int meshIndex = i; // �޽� �ε���
                part.partButton[i]?.onClick.AddListener(() => SetMesh(part, meshIndex));
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
