
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawingTool : MonoBehaviour
{
    /// <summary>
    /// The bread and butter of this whole operation...
    /// It controls the linerenderers and mousepositions and what not
    /// for when the user is drawing. You can use the variables under the Brush
    /// Settings header willy nilly to have some fun. Just changed the size of the
    /// Brush Colours list in the inspector and add what colours you want. Dont
    /// go too wild tho...
    /// </summary>

    // 기본 설정들.
    [Header("Setup Variables")]
    [SerializeField] private GameObject backgroundImage;    // 배경 이미지
    [SerializeField] private Transform drawnLinesParent;    // 그림이 그려지는 부모 오브젝트
    [SerializeField] private GameObject linePrefab;         // 그림을 그리기 위한 프리팹

    [SerializeField] private float lineStepDistance; // 마우스 이동 거리, 새로운 포인터

    private GameObject currentLine = null; // 현재 그리고 있는 라인 참조.
    private bool drawing = false; // 그리기 상태 여부를 나타냄
    private bool waitingToDraw = false; // 마우스가 캔버스 영역을 벗어나있지만 버튼이 눌려있는지 체크.
    private int layerOrder = 0; // 각 라인이 서로 겹치지 않도록 레이어 순서 지정.

    private List<GameObject> allDrawnLines = new List<GameObject>();    // 그려진 모든 라인
    private List<Vector3> currentLinePositions = new List<Vector3>(); // 현재 그리고 있는 라인의 모든 포지션 업데이트

    // 브러쉬 설정들
    [Header("Brush Settings")]
    [Range(0.01f, 0.5f)] public float brushSize = 0.5f; // 브러쉬 크기
    public int currentMaterialIndex = 0;    // 현재 사용 중인 브러쉬 색상
    public List<Color> brushColours = new List<Color>(); // 브러쉬 색상 리스트
    private List<Material> brushMaterals = new List<Material>(); // 브러쉬 material 리스트

    public Image img;

    private void Start()
    {
        // 브러쉬 색상 테스트
        for (int i = 0; i < brushColours.Count; i++)
        {
            Material newColour = new Material(Shader.Find("Sprites/Default"));
            newColour.color = brushColours[i];
            Debug.Log(newColour);
            brushMaterals.Add(newColour);
        }
    }


    private void Update()
    {
        //print(EventSystem.current.currentSelectedGameObject.name);
        if (InBounds())
        {
            // 그리기 실력
            if (Input.GetMouseButtonDown(0))
            {
                CreateLine();
            }
            // 그리는 중
            if (drawing && Input.GetMouseButton(0))
            {
                AddToLine();
                DrawLine();
            }
            // 그리기 종료
            if (Input.GetMouseButtonUp(0))
            {
                EndLine();
            }

            // 마우스가 캔버스로 돌아오면 그림을 그리기 시작.
            if (waitingToDraw && Input.GetMouseButton(0))
            {
                waitingToDraw = false;
                CreateLine();
            }
        }

        // 그림을 그리는 중에 캔버스를 벗어날 경우 그리기 종료
        if (!InBounds() && drawing)
        {
            EndLine();
        }

        // 마우스가 눌린 상태에서 캔버스를 벗어나면 waitingToDraw를 활성화
        if (!InBounds() && Input.GetMouseButton(0))
        {
            waitingToDraw = true;
        }
        if (!InBounds() && !Input.GetMouseButton(0))
        {
            waitingToDraw = false;
        }
    }

    // 마우스 위치가 캔버스 내에 있는지 여부 반환
    private bool InBounds()
    {
        //Get our mouse position

        if (Camera.main == null)
            return false;

        var mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePosFinal = new Vector3(mousePosRaw.x, mousePosRaw.y, drawnLinesParent.position.z);
        //Get the width and height of the sprite (dont forget the scale of our image object)
        var width = backgroundImage.GetComponent<SpriteRenderer>().size.x * backgroundImage.transform.localScale.x * 0.95f;
        var height = backgroundImage.GetComponent<SpriteRenderer>().size.y * backgroundImage.transform.localScale.y * 0.95f;
        var backTrans = backgroundImage.transform.position; //Get reference to the position of the image
                                                            //Check if mouse is inside its bounds
        if ((mousePosFinal.x > backTrans.x - width / 2 && mousePosFinal.x < backTrans.x + width / 2) && (mousePosFinal.y > backTrans.y - height / 2 && mousePosFinal.y < backTrans.y + height / 2))
        {

            return true;
        }

        return false;
    }

    // 그림을 그리기 시작
    private void CreateLine()
    {
#if UNITY_ANDROID
		Touch touch = Input.GetTouch(0);
		if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

#else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
#endif


        drawing = true;
        //Create the line at mouse position and add positions to line
        var mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse position to world
        var mousePosFinal = new Vector3(mousePosRaw.x, mousePosRaw.y, drawnLinesParent.position.z);
        currentLine = Instantiate(linePrefab, mousePosFinal, Quaternion.identity, drawnLinesParent); //Instantiate the line at this position
        allDrawnLines.Add(currentLine); //Store this new line in our array 
                                        //Add the mouse position to the start of the line, adds twice to solve an error but dw about it ;)
        currentLinePositions.Add(mousePosFinal);
        currentLinePositions.Add(mousePosFinal);

        //Set brush settings
        var currentRend = currentLine.GetComponent<LineRenderer>();
        currentRend.startWidth = brushSize;
        currentRend.material = brushMaterals[currentMaterialIndex];
        currentRend.sortingOrder = layerOrder;
        layerOrder++;


    }

    // draw를 업데이트하여 실시간으로 보여줌.
    private void DrawLine()
    {
        var renderer = currentLine.GetComponent<LineRenderer>();//Current line renderer we are using
        renderer.positionCount = currentLinePositions.Count; //Set its position count to be the same as how many positions we have entered
                                                             //Loop through all of our positions and add them to the renderer
        for (int i = 0; i < currentLinePositions.Count; i++)
        {
            renderer.SetPosition(i, currentLinePositions[i]);
        }
    }

    // 마지막 추가된 포인트와의 거리 확인, 일정 거리 이상일 경우 draw, 리스트에 추가.
    private void AddToLine()
    {
        //Get the mouse position but on the same Z pos as the parent object the lines get placed under
        var mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse position to world
        var mousePosFinal = new Vector3(mousePosRaw.x, mousePosRaw.y, drawnLinesParent.position.z);

        //Get the last added position and check the distance between the mouse and that position
        var currentIndex = currentLinePositions.Count;
        if (Vector3.Distance(currentLinePositions[currentIndex - 1], mousePosFinal) > lineStepDistance)
        {
            currentLinePositions.Add(mousePosFinal);
        }
    }

    // 그리기를 종료하고 초기화
    private void EndLine()
    {
        drawing = false;
        currentLine = null;
        currentLinePositions.Clear();
    }

    // 마지막으로 그린 라인을 삭제.
    public void Undo()
    {
        if (allDrawnLines.Count > 0)
        {
            var toDestroy = allDrawnLines[allDrawnLines.Count - 1];
            allDrawnLines.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }

    // 브러쉬 크기 업데이트
    public void UpdateBrushWidth(float newWidth)
    {
        brushSize = newWidth;
    }

    // 브러쉬 색상을 업데이트
    public void UpdateBrushColour(int newColour)
    {
        //Set the colour of the brush (used in conjunction with the DrawingToolUiController.cs script)
        if (newColour >= brushColours.Count)
        {
            currentMaterialIndex = 0;
            Debug.LogWarning("Trying to find colour index of " + newColour + " which is not possible...");
        }
        else
        {
            currentMaterialIndex = newColour;
            // 이미지 업데이트
            img.color = brushColours[newColour];
        }
    }

    // 캔버스를 초기화.
    public void ClearCanvas()
    {
        EndLine();
        while (allDrawnLines.Count > 0)
        {
            var toDestroy = allDrawnLines[allDrawnLines.Count - 1];
            allDrawnLines.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }



}
