using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherWork : MonoBehaviour
{
    private Button btn_Work;

    private void Awake()
    {
        btn_Work = GetComponent<Button>();

        btn_Work.onClick.AddListener(() => OnButtonClick());
    }

    private void OnButtonClick()
    {
        // 선생님이 하는 일로의 작업 전환
        print("!");
    }
}
