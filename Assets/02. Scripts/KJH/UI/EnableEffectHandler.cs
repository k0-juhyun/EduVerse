using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnableEffectHandler : MonoBehaviour, IPointerEnterHandler
{
    private Sequence mySequence;
    private Vector3 originPos;

    private void Awake()
    {
        originPos = this.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOMove(originPos, 0.5f, true);
    }
}
