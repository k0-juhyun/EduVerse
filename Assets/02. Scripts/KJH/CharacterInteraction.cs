using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

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

    private bool isSit;

    private void Awake()
    {
        anim = Character.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        // ���� ��ó�� �ְ�
        if (other.gameObject.CompareTag("Chair"))
        {
            ChairInfo chairInfo = other.GetComponent<ChairInfo>();
            isSit = chairInfo.isFull;

            // ���� ������ ��ư�� Ŭ��������
            if (EventSystem.current.currentSelectedGameObject == Btn_Sit.gameObject)
            {
                // ���ڿ� ���� �ɾ� ���� ������
                // isFull = false �̸� ����ġ�� ���� �ɴ� �ִϸ��̼� ����
                if (false == isSit)
                {
                    // �ɴ� �ִϸ��̼ǽ����ϰ�
                    anim.Play("Sit");

                    // ��ġ�� ���� ��ġ��
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
