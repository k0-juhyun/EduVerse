using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 상호작용
// 1. 버튼 누르면 의자에 앉기
// - 가장 가까운 의자 찾아서 앉기
// - 의자와 거리가 가까우면 버튼 누를 수 있도록
// - 중복해서 앉을 수 없도록
public class CharacterInteraction : MonoBehaviour
{
    public Button Btn_Sit;

    public GameObject Character;
    private Animator anim;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
    }

    public class ChairInfo
    {
        public string chairName;
        public bool isSit; // 앉았는지 여부
    }

    private List<ChairInfo> chairs = new List<ChairInfo>(); // 의자 정보 리스트

    private void Start()
    {
        Btn_Sit.onClick.AddListener(SitOnChair);
        print("?");
    }

    // 의자에 앉기 버튼을 누르면 호출될 함수
    private void SitOnChair()
    {
        anim.Play("Sit");
        // 가장 가까운 의자 찾기
        ChairInfo nearestChair = FindNearestChair();

        // 의자가 있다면 앉히기
        if (nearestChair != null && !nearestChair.isSit)
        {
            SitCharacter(nearestChair);
        }
    }

    // 가장 가까운 의자 찾기
    private ChairInfo FindNearestChair()
    {
        // 여기에 가장 가까운 의자를 찾는 코드를 추가하세요.
        return null; // 임시로 null을 반환하도록 설정합니다.
    }

    // 캐릭터를 의자에 앉히는 함수
    private void SitCharacter(ChairInfo chair)
    {
        // 캐릭터를 의자에 앉히는 코드를 추가하세요.
        // 해당 의자에 캐릭터를 앉히고 isSit 값을 true로 변경합니다.
    }
}
