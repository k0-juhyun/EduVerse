using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

// 1. 의자에 앉기
// - 히어라키창에 있는 의자 오브젝트를 태그를 이용해서 찾아서
// - 의자 리스트를 만들어서 리스트에 넣고
// - 리스트에 있는 의자 오브젝트 중에서
// - 캐릭터와 가장 가까운 의자를 찾아서
// - 앉기 버튼을 누르면 앉게 한다.
// - 다른 사람이 앉아있는 의자는 앉지 못한다.

public class CharacterInteraction : MonoBehaviour
{
    public GameObject Character;
    [Space]
    public Button Btn_Sit;
    private Animator anim;

    private bool isSit;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        // 의자 근처에 있고
        if (other.gameObject.CompareTag("Chair"))
        {
            ChairInfo chairInfo = other.GetComponent<ChairInfo>();
            isSit = chairInfo.isFull;

            // 범위 내에서 버튼을 클릭했을때
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
            {
                // 의자에 누가 앉아 있지 않으면
                // isFull = false 이면 그위치로 가서 앉는 애니메이션 실행
                if (false == isSit)
                {
                    // 앉는 애니메이션실행하고
                    anim.Play("Sit");

                    // 위치를 의자 위치로
                    Character.transform.position =
                        new Vector3(other.transform.position.x, 0.2f, other.transform.position.z);
                    Character.transform.forward = -other.transform.right;
                }
            }
            else
            {
                Character.transform.position = new Vector3
                    (Character.transform.position.x, 0, Character.transform.position.z);
            }
        }
    }
}
