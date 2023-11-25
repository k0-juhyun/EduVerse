using DG.Tweening;
using UnityEngine;

public class EnableEffectHandler : MonoBehaviour
{
    private Sequence mySequence;

    private void OnEnable()
    {
        StartBounceEffect();
    }

    private void OnDisable()
    {
        if (mySequence != null)
        {
            mySequence.Kill();
        }
    }

    private void StartBounceEffect()
    {
        float bounceHeight = 0.5f; // Ƣ�� ����
        float duration = 1f; // �� Ƣ�� ������ ���� �ð�
        int bounces = 5; // Ƣ�� Ƚ��

        mySequence = DOTween.Sequence();

        for (int i = 0; i < bounces; i++)
        {
            // ���� Ƣ�� ȿ��
            mySequence.Append(transform.DOLocalMoveY(bounceHeight, duration).SetEase(Ease.OutQuad))
                      .Append(transform.DOLocalMoveY(0, duration).SetEase(Ease.InQuad));

        }

        mySequence.Play();
    }
}
