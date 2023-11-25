using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StartStudy : MonoBehaviour
{
    public Button Btn_study;

    public bool _isTeacherSit;
    public bool _isDrawing;
    public bool isClick = false;
    public bool enableCanvas = true;

    public CameraSetting _cameraSetting;
    public CharacterInteraction characterInteraction;
    private void Start()
    {
        Btn_study.onClick.AddListener(() => OnStudyButtonClick());
    }

    public void OnStudyButtonClick()
    {
        enableCanvas = false;
        _isDrawing = true;
        _cameraSetting.TPS_Camera.gameObject.SetActive(false);
        isClick = true; // ��ư Ŭ�� ���¸� true�� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            _cameraSetting = other.gameObject.transform.parent.GetComponentInChildren<CameraSetting>();

            StartCoroutine(ICheckClickStudyButton(other));
            StartCoroutine(ICheckSit(other));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            _isTeacherSit = characterInteraction._isSit;
            if (Camera.main != null)
                Btn_study.transform.forward = Camera.main.transform.forward;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DataBase.instance.user.isTeacher)
        {
            Btn_study.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack).OnComplete(()
            => Btn_study.gameObject.SetActive(_isTeacherSit));
        }
    }

    private IEnumerator ICheckClickStudyButton(Collider other)
    {
        characterInteraction = other.gameObject.GetComponentInParent<CharacterInteraction>();

        // _isSit�� true�� �� ������ ��ٸ�
        yield return new WaitUntil(() => characterInteraction._isSit);

        _isTeacherSit = characterInteraction._isSit;
        characterInteraction.isDrawing = _isDrawing;

        // _isTeacherSit�� true�� �� ��ư�� Ȱ��ȭ�ϰ�, ������ �ִϸ��̼��� ����
        if (_isTeacherSit)
        {
            Btn_study.gameObject.SetActive(true);
            Btn_study.gameObject.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);
        }

        isClick = false;
    }

    private IEnumerator ICheckSit(Collider other)
    {
        characterInteraction = other.gameObject.GetComponentInParent<CharacterInteraction>();

        yield return new WaitUntil(() => characterInteraction._isSit == false);

        if (characterInteraction._isSit == false)
        {
            Btn_study.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InBack).OnComplete(()
            => Btn_study.gameObject.SetActive(_isTeacherSit));
        }
    }

}
