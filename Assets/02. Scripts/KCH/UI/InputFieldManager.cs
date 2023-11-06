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
        // �� Input Field�� ���� OnValueChanged �̺�Ʈ�� ����մϴ�.
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

    // �����
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
        // ���� Input Field�� �Էµ� �ؽ�Ʈ�� �����ɴϴ�.
        string currentText = inputFields[currentIndex].text;
        CurrendIndex = currentIndex;
        Debug.Log("OnInputValueChanged");

        // �Էµ� �ؽ�Ʈ�� �� ������ ���
        if (currentText.Length == 1)
        {
            // ���� Input Field�� ��Ŀ���� �̵��մϴ�.
            if (currentIndex < inputFields.Length - 1)
            {
                inputFields[currentIndex + 1].Select();
            }
        }
        else if (currentText.Length == 0)
        {
            Debug.Log("����");
            // �Էµ� �ؽ�Ʈ�� ���� ���
            // ���� Input Field�� ��Ŀ���� �̵��մϴ�.
            if (currentIndex > 0)
            {
                inputFields[currentIndex - 1].Select();
            }
        }
    }


}