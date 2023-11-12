using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Quiz_Individual : MonoBehaviourPun
{
    // ������ ������ �������� �������� üũ�ؼ� ������.
    public static Quiz_Individual instance;
    private void Awake()
    {
        instance = this;
    }

    // Quiz ��ũ��Ʈ���� �̺�Ʈ ������.

    //private void OnEnable()
    //{
    //    Quiz.instance.QuizEnded += OnQuizEnded; // �̺�Ʈ ����
    //}

    //private void OnDisable()
    //{
    //    Quiz.instance.QuizEnded -= OnQuizEnded; // �̺�Ʈ ����
    //}


     public void OnQuizEnded()
    {
        photonView.RPC(nameof(OX_GroundCheck), RpcTarget.All);
    }

    // rpc �� �ؾ߉�.
    [PunRPC]
    public void OX_GroundCheck()
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
        }
    }
 
}
