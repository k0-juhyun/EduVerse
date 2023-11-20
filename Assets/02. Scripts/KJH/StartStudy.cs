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

    public CameraSetting _cameraSetting;

    private void Start()
    {
        Btn_study.onClick.AddListener(() => OnStudyButtonClick());
    }

    public void OnStudyButtonClick()
    {
        enableCanvas = false;
        _isDrawing = true;
        _cameraSetting.TPS_Camera.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            print(other.gameObject.name);
            CharacterInteraction characterInteraction = other.gameObject.GetComponentInParent<CharacterInteraction>();
            _isTeacherSit = characterInteraction._isSit;
            characterInteraction.isDrawing = _isDrawing;

            Btn_study.gameObject.SetActive(_isTeacherSit);
            if (Camera.main != null)
            {
                Btn_study.gameObject.transform.forward = Camera.main.transform.forward;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            print(other.gameObject.transform.parent.name);
            _cameraSetting = other.gameObject.transform.parent.GetComponentInChildren<CameraSetting>();
            print(_cameraSetting);
        }
    }
}

