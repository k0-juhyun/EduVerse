using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInfo : MonoBehaviour
{
    // ���ڿ� �ɾ��ִ��� ����
    public bool isFull;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("dd");
        }
    }
}
