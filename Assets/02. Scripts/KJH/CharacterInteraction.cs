using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. ���ڿ� �ɱ�
// - �����Űâ�� �ִ� ���� ������Ʈ�� �±׸� �̿��ؼ� ã�Ƽ�
// - ���� ����Ʈ�� ���� ����Ʈ�� �ְ�
// - ����Ʈ�� �ִ� ���� ������Ʈ �߿���
// - ĳ���Ϳ� ���� ����� ���ڸ� ã�Ƽ�
// - �ɱ� ��ư�� ������ �ɰ� �Ѵ�.
// - �ٸ� ����� �ɾ��ִ� ���ڴ� ���� ���Ѵ�.


public class CharacterInteraction : MonoBehaviour
{
    public GameObject Character;
    [Space]
    public Button Btn_Sit;
    private Animator anim;

    public GameObject[] chairArray;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();

        chairArray = GameObject.FindGameObjectsWithTag("Chair");
    }

    private void Start()
    {
        
    }
}
