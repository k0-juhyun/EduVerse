using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartStudy : MonoBehaviour
{
    public Button Btn_study;

    public bool _isTeacherSit;
    public bool _isDrawing;
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
            _isDrawing = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            CharacterInteraction characterInteraction = other.gameObject.GetComponentInParent<CharacterInteraction>();
            _isTeacherSit = characterInteraction._isSit;
            characterInteraction.isDrawing = _isDrawing;

            Btn_study.gameObject.SetActive(_isTeacherSit && (isClick == false));
            if (Camera.main != null)
            {
                Btn_study.gameObject.transform.forward = Camera.main.transform.forward;
            }
        }
    }
}

