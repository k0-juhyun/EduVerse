using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopUpClickHandler : MonoBehaviour
{
    // 애니메이션 지속 시간
    public float animationDuration = 0.5f; 
    // 시작 스케일
    public Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f); 
    // 종료 스케일
    public Vector3 endScale = new Vector3(0.1f, 0.1f, 0.1f); 

    private void OnEnable()
    {
        // 팝업이 활성화될 때 시작 스케일에서 원래 스케일로 애니메이션 적용
        transform.localScale = startScale;
        transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack); // OutBack 이징으로 팝업 효과
    }

    private void OnDisable()
    {
        // 비활성화될 때 스케일 애니메이션
        transform.DOScale(endScale, animationDuration).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}

