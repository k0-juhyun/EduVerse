using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // ������ ������ �������� �������� üũ�ؼ� ������.


    private void Start()
    {
        Quiz.instance.QuizEnded += OnQuizEnded; // �̺�Ʈ ����
    }

    private void OnDisable()
    {
        Quiz.instance.QuizEnded -= OnQuizEnded; // �̺�Ʈ ����
    }

    void OnQuizEnded()
    {
        OX_GroundCheck();
    }

    void OX_GroundCheck()
    {
        Debug.Log("OX����");
        RaycastHit hit;
        float rayLength = 1.0f; // ���� ����
        Vector3 rayOrigin = transform.position; // �÷��̾��� ���� ��ġ
        Debug.Log(DataBase.instance.userInfo.name);
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength)&&!DataBase.instance.userInfo.isTeacher)
        {
            if (hit.collider.name == "O")
            {
                Debug.Log("O");
                // �������� üũ�ؼ� ������ �����ִ� �κ�
                
            }
            else if(hit.collider.name == "X")
            {
                Debug.Log("X");
                // �������� üũ�ؼ� ������ �����ִ� �κ�  
            }
            Debug.Log(hit.collider.name);
        }
    }
 
}
