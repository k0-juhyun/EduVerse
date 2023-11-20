using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InteractableModel : MonoBehaviourPun
{
    private Camera mainCamera;
    private bool isDragging;

    private float distanceToCamera;
    private float dragTime;
    private float activationTime = 4;

    public Image deleteCanvas;
    void Start()
    {
        mainCamera = Camera.main; // �� ī�޶� ĳ���մϴ�.
        distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺�� ������Ʈ ���� �ְ� Ŭ���ϸ� �巡�׸� �����մϴ�.
        if (Input.GetMouseButtonDown(0) && IsMouseOverObject())
        {
            isDragging = true;
            dragTime = 0f;
        }

        if (isDragging)
        {
            dragTime += Time.deltaTime; // �巡�� �ð��� ������Ʈ
            if (dragTime >= activationTime)
            {
                deleteCanvas.enabled = true; // ������ �ð� �̻� �巡�� �� delete �̹��� Ȱ��ȭ
            }

            // �巡�� �� ���콺 �̵��� ���� ������Ʈ ��ġ�� �����մϴ�.
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceToCamera; // ������ ī�޶���� �Ÿ��� �̿��մϴ�.
            Vector3 objPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;

            // ���콺�� ������ �巡�׸� ����ϴ�.
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                if (IsOverDeleteImage()) // delete �̹��� ���� ���Ҵ��� Ȯ��
                {
                    Destroy(gameObject); // ������Ʈ ����
                }
                deleteCanvas.enabled = false; // delete �̹��� ��Ȱ��ȭ
            }
        }

        // �� �Է��� �޾� ī�޶���� �Ÿ��� �����մϴ�.
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distanceToCamera -= scroll; // ��ũ�� ���⿡ ���� �Ÿ��� �����մϴ�.
        distanceToCamera = Mathf.Clamp(distanceToCamera, 1f, 10f); // �Ÿ��� ������ �Ӵϴ�.
    }

    // ���콺�� ������Ʈ ���� �ִ��� Ȯ���ϴ� �޼����Դϴ�.
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

    private bool IsOverDeleteImage()
    {
        // delete �̹��� ���� �ִ��� Ȯ���ϴ� ���� ����
        return RectTransformUtility.RectangleContainsScreenPoint(deleteCanvas.rectTransform, Input.mousePosition, mainCamera);
    }

    public bool IsDragging()
    {
        return isDragging;
    }
}
