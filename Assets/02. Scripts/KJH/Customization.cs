using DG.Tweening.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 커스텀할 파츠 클래스
// 단일 쌍 파츠
[System.Serializable]
public class CustomPart
{
    public string partName; // 커스텀할 파츠 이름
    public GameObject partObj; // 커스텀할 파츠 오브젝트
    public string objName;
    public Mesh[] partList; // 교체 메쉬들
    [HideInInspector]
    public int currentIdx; // 현재 인덱스
    public SkinnedMeshRenderer customRenderer;
    public Button rightBtn;
    public Button leftBtn;
}
// 캐릭터 커스터마이징
// 버튼 누르면 각 커스터마이징 부위가 변경됨

public class Customization : MonoBehaviour
{
    [Header("목록")]
    public CustomPart[] customParts;

    private void Awake()
    {
        // 컴포넌트 할당
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

        // 리스트에 처음에 모든 메쉬들의 인덱스를 넣어두고
        for (int i = 0; i < customParts.Length; i++)
        {
            DataBase.instance.myInfo.meshObjName.Add(customParts[i].objName);
            DataBase.instance.myInfo.meshIndex.Add(customParts[i].currentIdx); // 요소 추가
        }
    }

    // 캐릭터 커스터마이징
    // 좌클릭, 우클릭으로 메쉬 변경
    private void Customize(ref int curIdx, Mesh[] allIdx, 
        ref SkinnedMeshRenderer curRenderer, bool isRight, int index)
    {
        // 좌클릭, 우클릭으로 인덱스 변경
        if (isRight)
        {
            curIdx = (curIdx + 1) % allIdx.Length;
        }
        else
        {
            curIdx = (curIdx - 1 + allIdx.Length) % allIdx.Length;
        }

        // 인덱스에 따라 메쉬 변경
        if (curRenderer != null && curIdx >= 0 && curIdx < allIdx.Length)
        {
            curRenderer.sharedMesh = allIdx[curIdx];
            DataBase.instance.myInfo.meshIndex[index] = curIdx;
        }
    }

    // 클릭 이벤트
    #region OnClickEvent
    // 우클릭
    public void OnClickRight(CustomPart part)
    {
        int index = System.Array.IndexOf(customParts, part);
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, true, index);
    }
    // 좌클릭
    public void OnClickLeft(CustomPart part)
    {
        int index = System.Array.IndexOf(customParts, part);
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, false, index);
    }
    #endregion
}
