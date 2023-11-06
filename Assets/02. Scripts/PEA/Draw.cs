using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draw : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    private bool isDrawing = false;

    #region 라인렌더러
    //private Camera cam;  //Gets Main Camera
    //public Material defaultMaterial; //Material for Line Renderer

    //private LineRenderer curLine;  //Line which draws now
    //private int positionCount = 2;  //Initial start and end position
    //private Vector3 PrevPos = Vector3.zero; // 0,0,0 position variable

    //public GameObject line;
    #endregion

    #region 렌더텍스쳐
    public int resolution = 512;
    [Range(0.01f, 1f)]
    public float brushSize = 0.1f;
    public Texture2D brushTexture;

    private Texture2D mainTex;
    private MeshRenderer mr;
    private RenderTexture rt;

    private RawImage rawImage;
    #endregion

    private void Awake()
    {
        #region 렌더텍스쳐
        TryGetComponent(out mr);
        rt = new RenderTexture(resolution, resolution, 32);

        //rawImage = GetComponent<RawImage>();

        if (mr.material.mainTexture != null)
        {
            mainTex = mr.material.mainTexture as Texture2D;
        }
        // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
        else
        {
            mainTex = new Texture2D(resolution, resolution);
        }

        // 메인 텍스쳐 -> 렌더 텍스쳐 복제
        Graphics.Blit(mainTex, rt);

        // 렌더 텍스쳐를 메인 텍스쳐에 등록
        mr.material.mainTexture = rt;
        //rawImage.texture = rt;

        // 브러시 텍스쳐가 없을 경우 임시 생성(red 색상)
        if (brushTexture == null)
        {
            brushTexture = new Texture2D(resolution, resolution);
            for (int i = 0; i < resolution; i++)
                for (int j = 0; j < resolution; j++)
                    brushTexture.SetPixel(i, j, Color.red);
            brushTexture.Apply();
        }
        #endregion
    }

    void Start()
    {
        //cam = Camera.main;
    }

    void Update()
    {
        //DrawMouse();

        #region 렌더텍스쳐
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool raycast = Physics.Raycast(ray, out var hit);
            Collider col = hit.collider;

            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            // 본인이 레이캐스트에 맞았으면 그리기
            if (raycast && col && col.transform == transform)
            {
                Vector2 pixelUV = hit.lightmapCoord;
                pixelUV *= resolution;
                DrawTexture(pixelUV);
            }
        }
        #endregion
    }

    #region 렌더텍스쳐
    public void DrawTexture(in Vector2 uv)
    {
        RenderTexture.active = rt; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
        GL.PushMatrix();                                  // 매트릭스 백업
        GL.LoadPixelMatrix(0, resolution, resolution, 0); // 알맞은 크기로 픽셀 매트릭스 설정

        float brushPixelSize = brushSize * resolution;

        // 렌더 텍스쳐에 브러시 텍스쳐를 이용해 그리기
        Graphics.DrawTexture(
            new Rect(
                uv.x - brushPixelSize * 0.5f,
                (rt.height - uv.y) - brushPixelSize * 0.5f,
                brushPixelSize,
                brushPixelSize
            ),
            brushTexture
        );

        GL.PopMatrix();              // 매트릭스 복구
        RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
    }
    #endregion

    public void OnPointerMove(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    #region 라인렌더러
    //private void DrawMouse()
    //{
    //    Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.3f));

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        CreateLine(mousePos);
    //    }
    //    else if (Input.GetMouseButton(0))
    //    {
    //        ConnectLine(mousePos);
    //    }

    //}

    //private void CreateLine(Vector3 mousePos)
    //{
    //    positionCount = 2;
    //    //GameObject line = new GameObject("Line");
    //    LineRenderer lineRend = Instantiate(line, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();

    //    //line.transform.parent = cam.transform;
    //    line.transform.position = mousePos;

    //    lineRend.startWidth = 0.01f;
    //    lineRend.endWidth = 0.01f;
    //    lineRend.numCornerVertices = 5;
    //    lineRend.numCapVertices = 5;
    //    //lineRend.material = defaultMaterial;
    //    lineRend.SetPosition(0, mousePos);
    //    lineRend.SetPosition(1, mousePos);

    //    curLine = lineRend;
    //}

    //void ConnectLine(Vector3 mousePos)
    //{
    //    if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
    //    {
    //        PrevPos = mousePos;
    //        positionCount++;
    //        curLine.positionCount = positionCount;
    //        curLine.SetPosition(positionCount - 1, mousePos);
    //    }

    //}
#endregion
}
