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
    private float distanceToCamera;
    private float dragTime;

    public Image deleteAreaImage; // 삭제 영역 이미지

    private const float activationTime = 3f; // 드래그 후 삭제 영역 활성화까지의 시간
    private const float disableTime = 2f; // 활성화 후 자동으로 비활성화까지의 시간

    void Start()
    {
        mesh = this.gameObject;
        mainCamera = Camera.main;
        distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
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
            mousePosition.z = distanceToCamera;
            Vector3 objPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                StopCoroutine(ActivateDeleteAreaAfterDelay()); // 드래그가 끝나면 코루틴 중단

                if (deleteAreaImage.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(
                    deleteAreaImage.rectTransform, Input.mousePosition, null))
                {
                    mesh.gameObject.transform.DOScale(0.1f,0.5f).SetEase(Ease.InQuart).OnComplete(() => PhotonNetwork.Destroy(mesh));
                }
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distanceToCamera -= scroll;
        distanceToCamera = Mathf.Clamp(distanceToCamera, 1f, 10f);
    }

    private IEnumerator ActivateDeleteAreaAfterDelay()
    {
        yield return new WaitForSeconds(activationTime);

        if (isDragging) // 3초 후 여전히 드래그 중이면 삭제 영역 활성화
        {
            deleteAreaImage.gameObject.SetActive(true);
            StartCoroutine(DeactivateDeleteAreaAfterDelay());
        }
    }

    private IEnumerator DeactivateDeleteAreaAfterDelay()
    {
        yield return new WaitForSeconds(disableTime);

        deleteAreaImage.gameObject.SetActive(false); // 2초 후 자동으로 비활성화
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
