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

    private bool _isSit;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
