using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviour
{
    // ���ڿ� �ɾ��ִ��� ����
    public bool isSit;

    // �ݶ��̴��� ���Դ��� Ȯ�ο���
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("dd");
        }
    }
}
