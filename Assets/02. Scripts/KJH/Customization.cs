using UnityEngine;
using UnityEngine.UI;

// ĳ���� Ŀ���͸���¡
// ��ư ������ �� Ŀ���͸���¡ ������ �����
public class Customization : MonoBehaviour
{
    [System.Serializable]
    // Ŀ������ ���� Ŭ����
    // ���� �� ����
    public class CustomPart
    {
        public string partName; // Ŀ������ ���� �̸�
        public GameObject partObj; // Ŀ������ ���� ������Ʈ
        public string objName;
        public Mesh[] partList; // ��ü �޽���
        [HideInInspector]
        public int currentIdx; // ���� �ε���
        public SkinnedMeshRenderer customRenderer;
        public Button rightBtn;
        public Button leftBtn;
    }

    [Header("���")]
    public CustomPart[] customParts;

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        foreach (var part in customParts)
        {
            part.objName = part.partObj.gameObject.name;
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;
            if(part.rightBtn != null && part.leftBtn != null)
            {
                part.rightBtn.onClick.AddListener(() => OnClickRight(part));
                part.leftBtn.onClick.AddListener(() => OnClickLeft(part));
            }
        }
    }

    // ĳ���� Ŀ���͸���¡
    // ��Ŭ��, ��Ŭ������ �޽� ����
    private void Customize(ref int curIdx, Mesh[] allIdx, 
        ref SkinnedMeshRenderer curRenderer, bool isRight)
    {
        // ��Ŭ��, ��Ŭ������ �ε��� ����
        if (isRight)
        {
            curIdx = (curIdx + 1) % allIdx.Length;
        }
        else
        {
            curIdx = (curIdx - 1 + allIdx.Length) % allIdx.Length;
        }

        // �ε����� ���� �޽� ����
        if (curRenderer != null && curIdx >= 0 && curIdx < allIdx.Length)
        {
            curRenderer.sharedMesh = allIdx[curIdx];
        }
    }

    // Ŭ�� �̺�Ʈ
    #region OnClickEvent
    // ��Ŭ��
    public void OnClickRight(CustomPart part)
    {
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, true);
    }
    // ��Ŭ��
    public void OnClickLeft(CustomPart part)
    {
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, false);
    }
    #endregion
}
