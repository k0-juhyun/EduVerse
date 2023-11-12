using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // 서버에 문제와 정답인지 오답인지 체크해서 보내줌.


    private void Start()
    {
        Quiz.instance.QuizEnded += OnQuizEnded; // 이벤트 구독
    }

    private void OnDisable()
    {
        Quiz.instance.QuizEnded -= OnQuizEnded; // 이벤트 해지
    }

    void OnQuizEnded()
    {
        OX_GroundCheck();
    }

    void OX_GroundCheck()
    {
        Debug.Log("OX실행");
        RaycastHit hit;
        float rayLength = 1.0f; // 레이 길이
        Vector3 rayOrigin = transform.position; // 플레이어의 현재 위치
        Debug.Log(DataBase.instance.userInfo.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.userInfo.isTeacher)
        {
            if (hit.collider.name == "O")
            {
                Debug.Log("O");
                // 정답인지 체크해서 서버에 보내주는 부분
                
            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // 오답인지 체크해서 서버에 보내주는 부분  
            }
            Debug.Log(hit.collider.name);
        }
    }
 
}
