using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopUpClickHandler : MonoBehaviour
{
    // �ִϸ��̼� ���� �ð�
    public float animationDuration = 0.5f; 
    // ���� ������
    public Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f); 
    // ���� ������
    public Vector3 endScale = new Vector3(0.1f, 0.1f, 0.1f); 

    private void OnEnable()
    {
        // �˾��� Ȱ��ȭ�� �� ���� �����Ͽ��� ���� �����Ϸ� �ִϸ��̼� ����
        transform.localScale = startScale;
        transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack); // OutBack ��¡���� �˾� ȿ��
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ�� �� ������ �ִϸ��̼�
        transform.DOScale(endScale, animationDuration).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}

