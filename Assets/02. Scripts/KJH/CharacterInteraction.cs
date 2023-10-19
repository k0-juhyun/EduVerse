using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. 의자에 앉기
// - 히어라키창에 있는 의자 오브젝트를 태그를 이용해서 찾아서
// - 의자 리스트를 만들어서 리스트에 넣고
// - 리스트에 있는 의자 오브젝트 중에서
// - 캐릭터와 가장 가까운 의자를 찾아서
// - 앉기 버튼을 누르면 앉게 한다.
// - 다른 사람이 앉아있는 의자는 앉지 못한다.

[System.Serializable]


public class CharacterInteraction : MonoBehaviour
{
    public GameObject Character;
    [Space]
    public Button Btn_Sit;
    private Animator anim;

    public GameObject[] chairArray;

    public Dictionary<bool, GameObject> chairObject;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();

        chairArray = GameObject.FindGameObjectsWithTag("Chair");
        for (int i = 0; i < chairArray.Length; i++)
        {
            chairObject.Add(false, chairArray[i]);
        }
    }
}
