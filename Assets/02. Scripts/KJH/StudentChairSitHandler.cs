using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

public class StudentChairSitHandler : MonoBehaviour
{
    public Button sitButton;
    private CharacterInteraction characterInteraction;

    [HideInInspector] public bool isOccupied = false;

    private void OnEnable()
    {
        if (characterInteraction != null)
        {
            characterInteraction.OnSitStatusChanged += UpdateButtonStatus;
        }
    }

    private void OnDisable()
    {
        if (characterInteraction != null)
        {
            characterInteraction.SetToIdleState();
            characterInteraction.OnSitStatusChanged -= UpdateButtonStatus;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            characterInteraction = other.GetComponentInParent<CharacterInteraction>();
            if (characterInteraction != null && characterInteraction.photonView.IsMine)
            {
                StartCoroutine(ICheckSit());
                sitButton.onClick.AddListener(() => characterInteraction.SitDownAtThisChair(gameObject,this));
                sitButton.gameObject.transform.localScale = Vector3.zero;

                if (!characterInteraction._isSit)
                {
                    sitButton.gameObject.SetActive(true);
                    sitButton.gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.InBack);
                    print("켜짐");
                }

                if (Camera.main != null)
                    sitButton.gameObject.transform.forward = Camera.main.transform.forward;

                characterInteraction.OnSitStatusChanged += UpdateButtonStatus;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (characterInteraction != null && characterInteraction.photonView.IsMine)
            {
                sitButton.onClick.RemoveAllListeners();
                sitButton.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InOutExpo)
                    .OnComplete(() => sitButton.gameObject.SetActive(false));
                print("꺼짐");
                characterInteraction.OnSitStatusChanged -= UpdateButtonStatus;
            }
        }
    }

    private void UpdateButtonStatus(bool isSitting)
    {
        if (!isSitting)
        {
            sitButton.gameObject.SetActive(true);
            sitButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack); // 스케일을 다시 1로 설정
        }
        else
        {
            sitButton.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InOutExpo)
                .OnComplete(() => sitButton.gameObject.SetActive(false)); // 스케일을 0.1로 설정하고 비활성화
        }
    }

    private IEnumerator ICheckSit()
    {
        yield return new WaitUntil(() => characterInteraction._isSit);

        if (characterInteraction._isSit)
        {
            sitButton.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InOutExpo)
                  .OnComplete(() => sitButton.gameObject.SetActive(false));
        }
    }
}

