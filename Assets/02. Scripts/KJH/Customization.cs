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

    private void Awake()
    {
        // 컴포넌트 할당 및 초기화
        foreach (var part in customParts)
        {
            part.objName = part.partObj.gameObject.name;
            part.customRenderer = part.partObj.GetComponent<SkinnedMeshRenderer>();
            part.currentIdx = 0;

            // 버튼 이벤트 할당
            for (int i = 0; i < part.partButton?.Length; i++)
            {
                int meshIndex = i; // 메쉬 인덱스
                part.partButton[i]?.onClick.AddListener(() => SetMesh(part, meshIndex));
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
