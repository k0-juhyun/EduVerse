using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quiz_Individual : MonoBehaviour
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
        RaycastHit hit;
        float rayLength = 1.0f; // ���� ����
        Vector3 rayOrigin = transform.position; // �÷��̾��� ���� ��ġ

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength))
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
        }
    }
 
}
