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

    public Image deleteAreaImage; // ���� ���� �̹���

    private const float activationTime = 3f; // �巡�� �� ���� ���� Ȱ��ȭ������ �ð�
    private const float disableTime = 2f; // Ȱ��ȭ �� �ڵ����� ��Ȱ��ȭ������ �ð�
    private const float scaleSpeed = 1.5f; // ũ�� ���� �ӵ�

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
        // PC���� ��ũ�� ���� ����� ũ�� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mesh.transform.localScale += Vector3.one * scroll * scaleSpeed;

        // ����Ͽ����� �� �հ��� ��ġ�� ũ�� ����
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

        // ũ�⸦ ���� ���� ���� ����
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
