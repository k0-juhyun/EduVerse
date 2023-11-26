using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeacherChairSitHandler : MonoBehaviour
{
    public Button sitButton;
    private CharacterInteraction characterInteraction;

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
            characterInteraction.OnSitStatusChanged -= UpdateButtonStatus;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && DataBase.instance.userInfo.isteacher)
        {
            characterInteraction = other.GetComponentInParent<CharacterInteraction>();
            if (characterInteraction != null)
            {
                StartCoroutine(ICheckSit());

                sitButton.onClick.AddListener(() => characterInteraction.SitDownTeacherChair(gameObject));
                sitButton.gameObject.transform.localScale = Vector3.zero;

                if (!characterInteraction._isSit)
                {
                    sitButton.gameObject.SetActive(true);
                    sitButton.gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
                }

                if (Camera.main != null)
                    sitButton.gameObject.transform.forward = Camera.main.transform.forward;

                characterInteraction.OnSitStatusChanged += UpdateButtonStatus;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && DataBase.instance.userInfo.isteacher)
        {
            sitButton.onClick.RemoveAllListeners();
            sitButton.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InQuart)
                .OnComplete(() => sitButton.gameObject.SetActive(false));
            if (characterInteraction != null)
            {
                characterInteraction.OnSitStatusChanged -= UpdateButtonStatus;
            }
        }
    }

    private void UpdateButtonStatus(bool isSitting)
    {
        if (!isSitting)
        {
            sitButton.gameObject.SetActive(true);
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

