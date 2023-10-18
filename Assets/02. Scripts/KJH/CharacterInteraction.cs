using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ĳ���� ��ȣ�ۿ�
// 1. ��ư ������ ���ڿ� �ɱ�
// - ���� ����� ���� ã�Ƽ� �ɱ�
// - ���ڿ� �Ÿ��� ������ ��ư ���� �� �ֵ���
// - �ߺ��ؼ� ���� �� ������
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
        public bool isSit; // �ɾҴ��� ����
    }

    private List<ChairInfo> chairs = new List<ChairInfo>(); // ���� ���� ����Ʈ

    private void Start()
    {
        Btn_Sit.onClick.AddListener(SitOnChair);
        print("?");
    }

    // ���ڿ� �ɱ� ��ư�� ������ ȣ��� �Լ�
    private void SitOnChair()
    {
        anim.Play("Sit");
        // ���� ����� ���� ã��
        ChairInfo nearestChair = FindNearestChair();

        // ���ڰ� �ִٸ� ������
        if (nearestChair != null && !nearestChair.isSit)
        {
            SitCharacter(nearestChair);
        }
    }

    // ���� ����� ���� ã��
    private ChairInfo FindNearestChair()
    {
        // ���⿡ ���� ����� ���ڸ� ã�� �ڵ带 �߰��ϼ���.
        return null; // �ӽ÷� null�� ��ȯ�ϵ��� �����մϴ�.
    }

    // ĳ���͸� ���ڿ� ������ �Լ�
    private void SitCharacter(ChairInfo chair)
    {
        // ĳ���͸� ���ڿ� ������ �ڵ带 �߰��ϼ���.
        // �ش� ���ڿ� ĳ���͸� ������ isSit ���� true�� �����մϴ�.
    }
}
