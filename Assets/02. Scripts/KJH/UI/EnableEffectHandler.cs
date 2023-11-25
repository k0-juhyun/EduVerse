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
        float bounceHeight = 0.5f; // 튀는 높이
        float duration = 1f; // 각 튀는 동작의 지속 시간
        int bounces = 5; // 튀는 횟수

        mySequence = DOTween.Sequence();

        for (int i = 0; i < bounces; i++)
        {
            // 통통 튀는 효과
            mySequence.Append(transform.DOLocalMoveY(bounceHeight, duration).SetEase(Ease.OutQuad))
                      .Append(transform.DOLocalMoveY(0, duration).SetEase(Ease.InQuad));

        }

        mySequence.Play();
    }
}
