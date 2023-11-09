using UnityEngine;

public class InteractableModel : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    private float distanceToCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; // 주 카메라를 캐시합니다.
        distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스가 오브젝트 위에 있고 클릭하면 드래그를 시작합니다.
        if (Input.GetMouseButtonDown(0) && IsMouseOverObject())
        {
            isDragging = true;
        }

        if (isDragging)
        {
            // 드래그 중 마우스 이동에 따라 오브젝트 위치를 갱신합니다.
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceToCamera; // 설정한 카메라와의 거리를 이용합니다.
            Vector3 objPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;

            // 마우스를 놓으면 드래그를 멈춥니다.
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        // 휠 입력을 받아 카메라와의 거리를 조절합니다.
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distanceToCamera -= scroll; // 스크롤 방향에 따라 거리를 조절합니다.
        distanceToCamera = Mathf.Clamp(distanceToCamera, 1f, 10f); // 거리에 제한을 둡니다.
    }

    // 마우스가 오브젝트 위에 있는지 확인하는 메서드입니다.
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
