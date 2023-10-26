using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartStudy : MonoBehaviour
{
    public Button Btn_study;

    public bool _isTeacherSit;
    public bool isClick = false;
    public bool enableCanvas = true;

    private void Start()
    {
        Btn_study.onClick.AddListener(OnStudyButtonClick);
    }

    private void OnStudyButtonClick()
    {
        if (_isTeacherSit && !isClick)
        {
            isClick = true;
            enableCanvas = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DataBase.instance.userInfo.isTeacher)
        {
            _isTeacherSit = other.gameObject.GetComponentInParent<CharacterInteraction>().isTeacherSit;

            Btn_study.gameObject.SetActive(_isTeacherSit && (isClick == false));
            Btn_study.gameObject.transform.forward = Camera.main.transform.forward;
        }
    }
}

