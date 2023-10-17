using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTest : MonoBehaviour
{
    public GameObject linePrefab; // ���� �׸� ������ ������ ������

    private GameObject currentLine;
    private LineRenderer lineRenderer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
            AddPointToLine();
        }

        if (Input.GetMouseButton(0))
        {
            AddPointToLine();
        }
    }

    void CreateNewLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, GetMouseWorldPosition());
    }

    void AddPointToLine()
    {
        if (lineRenderer != null)
        {
            int currentPositionCount = lineRenderer.positionCount;
            lineRenderer.positionCount = currentPositionCount + 1;
            lineRenderer.SetPosition(currentPositionCount, GetMouseWorldPosition());
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // ī�޶���� �Ÿ� ����
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
