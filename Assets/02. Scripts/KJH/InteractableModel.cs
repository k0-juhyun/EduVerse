using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using DG.Tweening;

public class InteractableModel : MonoBehaviourPun
{
    private GameObject mesh;
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 originalScale;
    private float dragTime;

    public Image deleteAreaImage; // 삭제 영역 이미지

    private const float activationTime = 3f; // 드래그 후 삭제 영역 활성화까지의 시간
    private const float disableTime = 2f; // 활성화 후 자동으로 비활성화까지의 시간
    private const float scaleSpeed = 1.5f; // 크기 조절 속도

    void Start()
    {
        mesh = this.gameObject;
        mainCamera = Camera.main;
        originalScale = mesh.transform.localScale;
        deleteAreaImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsMouseOverObject())
        {
            isDragging = true;
            dragTime = 0f;
            StartCoroutine(ActivateDeleteAreaAfterDelay());
        }

        if (isDragging)
        {
            dragTime += Time.deltaTime;

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.nearClipPlane;
            Vector3 objPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                StopCoroutine(ActivateDeleteAreaAfterDelay());

                if (deleteAreaImage.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(
                    deleteAreaImage.rectTransform, Input.mousePosition, null))
                {
                    mesh.gameObject.transform.DOScale(0.1f, 0.5f).SetEase(Ease.InQuart).OnComplete(() => PhotonNetwork.Destroy(mesh));
                }
            }
        }

        HandleScaleChange();
    }

    private IEnumerator ActivateDeleteAreaAfterDelay()
    {
        yield return new WaitForSeconds(activationTime);

        if (isDragging && DataBase.instance.userInfo.isteacher)
        {
            deleteAreaImage.gameObject.SetActive(true);
            StartCoroutine(DeactivateDeleteAreaAfterDelay());
        }
    }

    private IEnumerator DeactivateDeleteAreaAfterDelay()
    {
        yield return new WaitForSeconds(disableTime);
        deleteAreaImage.gameObject.SetActive(false);
    }

    private void HandleScaleChange()
    {
        // PC에서 스크롤 휠을 사용한 크기 조절
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mesh.transform.localScale += Vector3.one * scroll * scaleSpeed;

        // 모바일에서의 두 손가락 터치로 크기 조절
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            mesh.transform.localScale -= Vector3.one * deltaMagnitudeDiff * scaleSpeed * Time.deltaTime;
        }

        // 크기를 원래 범위 내로 유지
        mesh.transform.localScale = Vector3.Max(mesh.transform.localScale, originalScale * 0.5f);
        mesh.transform.localScale = Vector3.Min(mesh.transform.localScale, originalScale * 2f);
    }

    private bool IsMouseOverObject()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == this.transform;
        }
        return false;
    }

    public bool IsDragging()
    {
        return isDragging;
    }
}
