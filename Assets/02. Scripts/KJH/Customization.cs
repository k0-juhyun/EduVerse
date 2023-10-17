using UnityEngine;
using UnityEngine.UI;

// 캐릭터 커스터마이징
// 버튼 누르면 각 커스터마이징 부위가 변경됨
public class Customization : MonoBehaviour
{
    [System.Serializable]
    // 커스텀할 파츠 클래스
    // 단일 쌍 파츠
    public class CustomPart
    {
        public string partName; // 커스텀할 파츠 이름
        public GameObject partObj; // 커스텀할 파츠 오브젝트
        public Mesh[] partList; // 교체 메쉬들
        [HideInInspector]
        public int currentIdx; // 현재 인덱스
        [HideInInspector]
        public SkinnedMeshRenderer customRenderer;
        public Button rightBtn;
        public Button leftBtn;
    }

    [Header("목록")]
    public CustomPart[] customParts;

    private void Awake()
    {
        // 컴포넌트 할당
        foreach (var part in customParts)
        {
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;
            part.rightBtn.onClick.AddListener(() => OnClickRight(part));
            part.leftBtn.onClick.AddListener(() => OnClickLeft(part));
        }
    }

    // 캐릭터 커스터마이징
    // 좌클릭, 우클릭으로 메쉬 변경
    private void Customize(ref int curIdx, Mesh[] allIdx, 
        ref SkinnedMeshRenderer curRenderer, bool isRight)
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
        }
    }

    // 클릭 이벤트
    #region OnClickEvent
    // 우클릭
    public void OnClickRight(CustomPart part)
    {
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, true);
    }
    // 좌클릭
    public void OnClickLeft(CustomPart part)
    {
        Customize(ref part.currentIdx, part.partList, ref part.customRenderer, false);
    }
    #endregion
}
