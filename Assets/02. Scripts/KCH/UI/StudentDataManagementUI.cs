using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentDataManagementUI : MonoBehaviour
{
    public Image backimage;
    public GameObject studentquizdata;
    public GameObject feedback;

    // feedback 버튼을 눌렀을때 배경 바뀌게.
    public void quizdataBtnClick()
    {
        ChangeColor("#fff8b6");
        // 패널 변경도 해줘야함.

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
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor)) // 16진수 문자열을 Color로 변환합니다.
        {
            backimage.color = newColor; // 이미지의 컬러를 변경합니다.
        }
        else
        {
            Debug.LogError("Invalid color format"); // 유효하지 않은 색상 형식일 경우 에러 메시지를 출력합니다.
        }
    }
}
