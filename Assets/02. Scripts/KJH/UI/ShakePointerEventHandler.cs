using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShakePointerEventHandler : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Transform headTransform; // 머리 부분의 Transform
    public float swingDuration = 0.5f; // 흔들림 지속 시간
    public float swingAngle = 20.0f; // 흔들림 각도

    private Sequence swingSequence;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartSwingAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopSwingAnimation();
    }


    private void StartSwingAnimation()
    {
        swingSequence = DOTween.Sequence();
        swingSequence.Append(headTransform.DORotate(new Vector3(0, 0, swingAngle), swingDuration).SetEase(Ease.InOutSine));
        swingSequence.Append(headTransform.DORotate(new Vector3(0, 0, -swingAngle), swingDuration * 2).SetEase(Ease.InOutSine));
        swingSequence.Append(headTransform.DORotate(Vector3.zero, swingDuration).SetEase(Ease.InOutSine));
        swingSequence.SetLoops(-1, LoopType.Restart); // 무한 반복
        swingSequence.Play();
    }

    private void StopSwingAnimation()
    {
        swingSequence.Kill();
        headTransform.DORotate(Vector3.zero, swingDuration).SetEase(Ease.InOutSine); // 원래 위치로 돌아감
    }
}
