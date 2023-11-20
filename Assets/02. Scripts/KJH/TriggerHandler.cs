using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    private CharacterInteraction parentScript;

    void Start()
    {
        // �θ� ������Ʈ�� CharacterInteraction ��ũ��Ʈ ã��
        parentScript = GetComponentInParent<CharacterInteraction>();
    }

    void OnTriggerEnter(Collider other)
    {
        // �θ� ��ũ��Ʈ�� HandleTriggerEnter �޼ҵ� ȣ��
        parentScript?.HandleTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        // �θ� ��ũ��Ʈ�� HandleTriggerStay �޼ҵ� ȣ��
        parentScript?.HandleTriggerStay(other);
    }
}
