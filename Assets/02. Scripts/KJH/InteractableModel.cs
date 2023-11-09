using UnityEngine;

public class InteractableModel : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private float distanceToCamera;

    // Start is called before the first frame update
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
        }

        if (isDragging)
        {
            // �巡�� �� ���콺 �̵��� ���� ������Ʈ ��ġ�� �����մϴ�.
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceToCamera; // ������ ī�޶���� �Ÿ��� �̿��մϴ�.
            Vector3 objPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;

            // ���콺�� ������ �巡�׸� ����ϴ�.
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
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
}
