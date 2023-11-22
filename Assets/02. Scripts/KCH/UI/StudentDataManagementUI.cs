using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentDataManagementUI : MonoBehaviour
{
    public Image backimage;
    public GameObject studentquizdata;
    public GameObject feedback;

    // feedback ��ư�� �������� ��� �ٲ��.
    public void quizdataBtnClick()
    {
        ChangeColor("#fff8b6");
        // �г� ���浵 �������.

        feedback.SetActive(false);
        studentquizdata.SetActive(true);
    }
    public void feedbackBtnClick()
    {
        ChangeColor("#ffd96a");

        feedback.SetActive(true);
        studentquizdata.SetActive(false);
    }

    void ChangeColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor)) // 16���� ���ڿ��� Color�� ��ȯ�մϴ�.
        {
            backimage.color = newColor; // �̹����� �÷��� �����մϴ�.
        }
        else
        {
            Debug.LogError("Invalid color format"); // ��ȿ���� ���� ���� ������ ��� ���� �޽����� ����մϴ�.
        }
    }
}
