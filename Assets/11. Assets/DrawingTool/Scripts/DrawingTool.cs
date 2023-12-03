
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

    // �⺻ ������.
    [Header("Setup Variables")]
    [SerializeField] private GameObject backgroundImage;    // ��� �̹���
    [SerializeField] private Transform drawnLinesParent;    // �׸��� �׷����� �θ� ������Ʈ
    [SerializeField] private GameObject linePrefab;         // �׸��� �׸��� ���� ������

    [SerializeField] private float lineStepDistance; // ���콺 �̵� �Ÿ�, ���ο� ������

    private GameObject currentLine = null; // ���� �׸��� �ִ� ���� ����.
    private bool drawing = false; // �׸��� ���� ���θ� ��Ÿ��
    private bool waitingToDraw = false; // ���콺�� ĵ���� ������ ��������� ��ư�� �����ִ��� üũ.
    private int layerOrder = 0; // �� ������ ���� ��ġ�� �ʵ��� ���̾� ���� ����.

    private List<GameObject> allDrawnLines = new List<GameObject>();    // �׷��� ��� ����
    private List<Vector3> currentLinePositions = new List<Vector3>(); // ���� �׸��� �ִ� ������ ��� ������ ������Ʈ

    // �귯�� ������
    [Header("Brush Settings")]
    [Range(0.01f, 0.5f)] public float brushSize = 0.5f; // �귯�� ũ��
    public int currentMaterialIndex = 0;    // ���� ��� ���� �귯�� ����
    public List<Color> brushColours = new List<Color>(); // �귯�� ���� ����Ʈ
    private List<Material> brushMaterals = new List<Material>(); // �귯�� material ����Ʈ

    public Image img;

    private void Start()
    {
        // �귯�� ���� �׽�Ʈ
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
            // �׸��� �Ƿ�
            if (Input.GetMouseButtonDown(0))
            {
                CreateLine();
            }
            // �׸��� ��
            if (drawing && Input.GetMouseButton(0))
            {
                AddToLine();
                DrawLine();
            }
            // �׸��� ����
            if (Input.GetMouseButtonUp(0))
            {
                EndLine();
            }

            // ���콺�� ĵ������ ���ƿ��� �׸��� �׸��� ����.
            if (waitingToDraw && Input.GetMouseButton(0))
            {
                waitingToDraw = false;
                CreateLine();
            }
        }

        // �׸��� �׸��� �߿� ĵ������ ��� ��� �׸��� ����
        if (!InBounds() && drawing)
        {
            EndLine();
        }

        // ���콺�� ���� ���¿��� ĵ������ ����� waitingToDraw�� Ȱ��ȭ
        if (!InBounds() && Input.GetMouseButton(0))
        {
            waitingToDraw = true;
        }
        if (!InBounds() && !Input.GetMouseButton(0))
        {
            waitingToDraw = false;
        }
    }

    // ���콺 ��ġ�� ĵ���� ���� �ִ��� ���� ��ȯ
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

    // �׸��� �׸��� ����
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

    // draw�� ������Ʈ�Ͽ� �ǽð����� ������.
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

    // ������ �߰��� ����Ʈ���� �Ÿ� Ȯ��, ���� �Ÿ� �̻��� ��� draw, ����Ʈ�� �߰�.
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

    // �׸��⸦ �����ϰ� �ʱ�ȭ
    private void EndLine()
    {
        drawing = false;
        currentLine = null;
        currentLinePositions.Clear();
    }

    // ���������� �׸� ������ ����.
    public void Undo()
    {
        if (allDrawnLines.Count > 0)
        {
            var toDestroy = allDrawnLines[allDrawnLines.Count - 1];
            allDrawnLines.Remove(toDestroy);
            Destroy(toDestroy);
        }
    }

    // �귯�� ũ�� ������Ʈ
    public void UpdateBrushWidth(float newWidth)
    {
        brushSize = newWidth;
    }

    // �귯�� ������ ������Ʈ
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
            // �̹��� ������Ʈ
            img.color = brushColours[newColour];
        }
    }

    // ĵ������ �ʱ�ȭ.
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
