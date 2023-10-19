using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteTest : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Material brushMaterial;

    void Start()
    {
        brushMaterial = new Material(Shader.Find("Unlit/Texture"));
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
    }

    void Draw()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= renderTexture.width;
            pixelUV.y *= renderTexture.height;

            RenderTexture.active = renderTexture;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);
            brushMaterial.SetTexture("_MainTex", rend.sharedMaterial.mainTexture);
            brushMaterial.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.TexCoord2(0, 0);
            GL.Vertex3(pixelUV.x, pixelUV.y, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(pixelUV.x + 1, pixelUV.y, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(pixelUV.x + 1, pixelUV.y + 1, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(pixelUV.x, pixelUV.y + 1, 0);
            GL.End();

            GL.PopMatrix();
            RenderTexture.active = null;
        }
    }
}
