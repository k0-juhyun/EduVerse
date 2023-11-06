using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draw : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    private bool isDrawing = false;

    #region ���η�����
    //private Camera cam;  //Gets Main Camera
    //public Material defaultMaterial; //Material for Line Renderer

    //private LineRenderer curLine;  //Line which draws now
    //private int positionCount = 2;  //Initial start and end position
    //private Vector3 PrevPos = Vector3.zero; // 0,0,0 position variable

    //public GameObject line;
    #endregion

    #region �����ؽ���
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
        #region �����ؽ���
        TryGetComponent(out mr);
        rt = new RenderTexture(resolution, resolution, 32);

        //rawImage = GetComponent<RawImage>();

        if (mr.material.mainTexture != null)
        {
            mainTex = mr.material.mainTexture as Texture2D;
        }
        // ���� �ؽ��İ� ���� ���, �Ͼ� �ؽ��ĸ� �����Ͽ� ���
        else
        {
            mainTex = new Texture2D(resolution, resolution);
        }

        // ���� �ؽ��� -> ���� �ؽ��� ����
        Graphics.Blit(mainTex, rt);

        // ���� �ؽ��ĸ� ���� �ؽ��Ŀ� ���
        mr.material.mainTexture = rt;
        //rawImage.texture = rt;

        // �귯�� �ؽ��İ� ���� ��� �ӽ� ����(red ����)
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

        #region �����ؽ���
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool raycast = Physics.Raycast(ray, out var hit);
            Collider col = hit.collider;

            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);

            // ������ ����ĳ��Ʈ�� �¾����� �׸���
            if (raycast && col && col.transform == transform)
            {
                Vector2 pixelUV = hit.lightmapCoord;
                pixelUV *= resolution;
                DrawTexture(pixelUV);
            }
        }
        #endregion
    }

    #region �����ؽ���
    public void DrawTexture(in Vector2 uv)
    {
        RenderTexture.active = rt; // �������� ���� Ȱ�� ���� �ؽ��� �ӽ� �Ҵ�
        GL.PushMatrix();                                  // ��Ʈ���� ���
        GL.LoadPixelMatrix(0, resolution, resolution, 0); // �˸��� ũ��� �ȼ� ��Ʈ���� ����

        float brushPixelSize = brushSize * resolution;

        // ���� �ؽ��Ŀ� �귯�� �ؽ��ĸ� �̿��� �׸���
        Graphics.DrawTexture(
            new Rect(
                uv.x - brushPixelSize * 0.5f,
                (rt.height - uv.y) - brushPixelSize * 0.5f,
                brushPixelSize,
                brushPixelSize
            ),
            brushTexture
        );

        GL.PopMatrix();              // ��Ʈ���� ����
        RenderTexture.active = null; // Ȱ�� ���� �ؽ��� ����
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

    #region ���η�����
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
