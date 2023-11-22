using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffectHandler : MonoBehaviour
{
    public float bounceDuration = 0.2f; // 바운스 애니메이션 지속 시간
    public Vector3 bounceScale = new Vector3(1.1f, 0.9f, 1.1f); // 바운스 시 최대 스케일
    private Vector3 originalScale; // 원래 스케일 값

    private void Start()
    {
        originalScale = transform.localScale; // 초기 스케일 저장
        StartCoroutine(IBounceEffect());
    }

    private IEnumerator IBounceEffect()
    {
        while (true)
        {
            yield return BounceEffect(); // 바운스 효과 코루틴 호출
        }
    }

    private IEnumerator BounceEffect()
    {
        // 커지는 효과
        yield return transform.DOScale(bounceScale, bounceDuration).SetEase(Ease.OutQuad).WaitForCompletion();

        // 바운스 효과와 함께 원래 크기로 돌아가는 효과
        yield return transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce).WaitForCompletion();
    }
}
