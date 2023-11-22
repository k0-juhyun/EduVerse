using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffectHandler : MonoBehaviour
{
    public float bounceDuration = 0.2f; // �ٿ �ִϸ��̼� ���� �ð�
    public Vector3 bounceScale = new Vector3(1.1f, 0.9f, 1.1f); // �ٿ �� �ִ� ������
    private Vector3 originalScale; // ���� ������ ��

    private void Start()
    {
        originalScale = transform.localScale; // �ʱ� ������ ����
        StartCoroutine(IBounceEffect());
    }

    private IEnumerator IBounceEffect()
    {
        while (true)
        {
            yield return BounceEffect(); // �ٿ ȿ�� �ڷ�ƾ ȣ��
        }
    }

    private IEnumerator BounceEffect()
    {
        // Ŀ���� ȿ��
        yield return transform.DOScale(bounceScale, bounceDuration).SetEase(Ease.OutQuad).WaitForCompletion();

        // �ٿ ȿ���� �Բ� ���� ũ��� ���ư��� ȿ��
        yield return transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce).WaitForCompletion();
    }
}
