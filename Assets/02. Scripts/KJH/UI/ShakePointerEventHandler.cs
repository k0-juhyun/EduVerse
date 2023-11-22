using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShakePointerEventHandler : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Transform headTransform; // �Ӹ� �κ��� Transform
    public float swingDuration = 0.5f; // ��鸲 ���� �ð�
    public float swingAngle = 20.0f; // ��鸲 ����

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
        swingSequence.SetLoops(-1, LoopType.Restart); // ���� �ݺ�
        swingSequence.Play();
    }

    private void StopSwingAnimation()
    {
        swingSequence.Kill();
        headTransform.DORotate(Vector3.zero, swingDuration).SetEase(Ease.InOutSine); // ���� ��ġ�� ���ư�
    }
}
