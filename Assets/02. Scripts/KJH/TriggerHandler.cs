using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerHandler : MonoBehaviour
{
    private CharacterInteraction parentScript;
    public Button sitBtn;

    void Start()
    {
        // �θ� ������Ʈ�� CharacterInteraction ��ũ��Ʈ ã��
        parentScript = GetComponentInParent<CharacterInteraction>();
    }

    void OnTriggerEnter(Collider other)
    {
        // �θ� ��ũ��Ʈ�� HandleTriggerEnter �޼ҵ� ȣ��
        parentScript?.HandleTriggerEnter(other);

        if(other.gameObject.tag == "Chair")
            sitBtn = other.GetComponent<StudentChairSitHandler>().sitButton;
        else if(other.gameObject.name == "Teacher Chair")
            sitBtn = other.GetComponent<TeacherChairSitHandler>().sitButton;
    }

    void OnTriggerStay(Collider other)
    {
        // �θ� ��ũ��Ʈ�� HandleTriggerStay �޼ҵ� ȣ��
        parentScript?.HandleTriggerStay(other);
    }
}
