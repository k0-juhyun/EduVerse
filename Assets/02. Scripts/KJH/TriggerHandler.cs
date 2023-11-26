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
        // 부모 오브젝트의 CharacterInteraction 스크립트 찾기
        parentScript = GetComponentInParent<CharacterInteraction>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 부모 스크립트의 HandleTriggerEnter 메소드 호출
        parentScript?.HandleTriggerEnter(other);

        if(other.gameObject.tag == "Chair")
            sitBtn = other.GetComponent<StudentChairSitHandler>().sitButton;
        else if(other.gameObject.name == "Teacher Chair")
            sitBtn = other.GetComponent<TeacherChairSitHandler>().sitButton;
    }

    void OnTriggerStay(Collider other)
    {
        // 부모 스크립트의 HandleTriggerStay 메소드 호출
        parentScript?.HandleTriggerStay(other);
    }
}
