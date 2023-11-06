using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class InputFieldManager : MonoBehaviour
{
    public InputField[] inputFields;

    int CurrendIndex = 0;

    void Start()
    {
        // 각 Input Field에 대해 OnValueChanged 이벤트를 등록합니다.
        for (int i = 0; i < inputFields.Length; i++)
        {
            int currentIndex = i;

            inputFields[i].onValueChanged.AddListener((value) =>
            {
                OnInputValueChanged(currentIndex);
                CurrendIndex = currentIndex;
            });
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnBackspacePressed();
        }
    }

    // 지우기
    private void OnBackspacePressed()
    {
        string currentText = inputFields[CurrendIndex].text;
        if(!string.IsNullOrEmpty(currentText))
        {
            
            inputFields[CurrendIndex].Select();

        }
        else
        {

        }
    }

    void OnInputValueChanged(int currentIndex)
    {
        // 현재 Input Field에 입력된 텍스트를 가져옵니다.
        string currentText = inputFields[currentIndex].text;
        CurrendIndex = currentIndex;
        Debug.Log("OnInputValueChanged");

        // 입력된 텍스트가 한 글자인 경우
        if (currentText.Length == 1)
        {
            // 다음 Input Field로 포커스를 이동합니다.
            if (currentIndex < inputFields.Length - 1)
            {
                inputFields[currentIndex + 1].Select();
            }
        }
        else if (currentText.Length == 0)
        {
            Debug.Log("실행");
            // 입력된 텍스트가 없는 경우
            // 이전 Input Field로 포커스를 이동합니다.
            if (currentIndex > 0)
            {
                inputFields[currentIndex - 1].Select();
            }
        }
    }


}