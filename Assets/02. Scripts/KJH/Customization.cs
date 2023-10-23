using DG.Tweening.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ŀ������ ���� Ŭ����
// ���� �� ����
[System.Serializable]
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
// ĳ���� Ŀ���͸���¡
// ��ư ������ �� Ŀ���͸���¡ ������ �����

public class Customization : MonoBehaviour
{
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

        // ����Ʈ�� ó���� ��� �޽����� �ε����� �־�ΰ�
        for (int i = 0; i < customParts.Length; i++)
        {
            DataBase.instance.myInfo.meshObjName.Add(customParts[i].objName);
            DataBase.instance.myInfo.meshIndex.Add(customParts[i].currentIdx); // ��� �߰�
        }
    }

    // ĳ���� Ŀ���͸���¡
    // ��Ŭ��, ��Ŭ������ �޽� ����
    private void Customize(ref int curIdx, Mesh[] allIdx, 
        ref SkinnedMeshRenderer curRenderer, bool isRight, int index)
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
            DataBase.instance.myInfo.meshIndex[index] = curIdx;
        }
    }

    // Ŭ�� �̺�Ʈ
    #region OnClickEvent
    // ��Ŭ��
    public void OnClickRight(CustomPart part)
    {
        int index = System.Array.IndexOf(customParts, part);
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, true, index);
    }
    // ��Ŭ��
    public void OnClickLeft(CustomPart part)
    {
        int index = System.Array.IndexOf(customParts, part);
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, false, index);
    }
    #endregion
}
