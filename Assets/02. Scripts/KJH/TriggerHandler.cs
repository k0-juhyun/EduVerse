using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    private CharacterInteraction parentScript;

    void Start()
    {
        // 부모 오브젝트의 CharacterInteraction 스크립트 찾기
        parentScript = GetComponentInParent<CharacterInteraction>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 부모 스크립트의 HandleTriggerEnter 메소드 호출
        parentScript?.HandleTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        // 부모 스크립트의 HandleTriggerStay 메소드 호출
        parentScript?.HandleTriggerStay(other);
    }
}
