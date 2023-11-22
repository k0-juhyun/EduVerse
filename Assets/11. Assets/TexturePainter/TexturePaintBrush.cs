// https://www.patreon.com/posts/rendertexture-15961186
// https://pastebin.com/rMx1PVXi

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 날짜 : 2021-06-18 AM 2:30:31
// 작성자 : Rito

namespace Rito.TexturePainter
{
    /// <summary> 마우스 드래그로 텍스쳐에 그림 그리기 </summary>
    [DisallowMultipleComponent]
    public class TexturePaintBrush : MonoBehaviour
    {
        private bool isDrawing = false;

        private Texture2D defaultTexture;
        public FlexibleColorPicker fcp;
        public Button brushBtn;
        public Button eraseBtn;
        public Texture2D eraseTexture;
        public Scrollbar brushWidthScrollbar;

        /***********************************************************************
        *                               Public Fields
        ***********************************************************************/
        #region .

        [Range(0.01f, 1f)] public float brushSize = 0.1f;
        public Texture2D brushTexture;
        public Color brushColor = Color.white;

        #endregion
        /***********************************************************************
        *                               Private Fields
        ***********************************************************************/
        #region .

        private TexturePaintTarget paintTarget;
        private Collider prevCollider;

        private Texture2D CopiedBrushTexture; // 실시간으로 색상 칠하는데 사용되는 브러시 텍스쳐 카피본
        private Vector2 sameUvPoint; // 직전 프레임에 마우스가 위치한 대상 UV 지점 (동일 위치에 중첩해서 그리는 현상 방지)

        #endregion

        /***********************************************************************
        *                               Unity Events
        ***********************************************************************/
        #region .
        private void Awake()
        {
            // 등록한 브러시 텍스쳐가 없을 경우, 원 모양의 텍스쳐 생성
            if (brushTexture == null)
            {
                CreateDefaultBrushTexture();
            }

            CopyBrushTexture();
        }

        private void Start()
        {
            fcp.onColorChange.AddListener(OnChangeColor);
            brushBtn.onClick.AddListener(OnClickBrushBtn);
            eraseBtn.onClick.AddListener(OnClickEraseBtn);
            brushWidthScrollbar.onValueChanged.AddListener((f) => OnBrushWidthScollbarValueChanged(f));
        }

        private void Update()
        {
            UpdateBrushColorOnEditor();

            if (Input.GetMouseButton(0) == false)
            {
                isDrawing = false;
                return;
            }

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit)) // delete previous and uncomment for mouse painting
            {
                Collider currentCollider = hit.collider;
                if (currentCollider != null)
                {
                    // 대상 참조 갱신
                    if (prevCollider == null || prevCollider != currentCollider)
                    {
                        prevCollider = currentCollider;
                        currentCollider.TryGetComponent(out paintTarget);
                        print(paintTarget.name);
                    }

                    // 동일한 지점에는 중첩하여 다시 그리지 않음
                    if (sameUvPoint != hit.textureCoord)
                    {
                        Vector2 pixelUV = hit.textureCoord;
                        pixelUV.x *= paintTarget.resolution;
                        pixelUV.y *= paintTarget.resolution;

                        Vector2 prevUV = new Vector2(sameUvPoint.x *= paintTarget.resolution, sameUvPoint.y *= paintTarget.resolution);
                        if(isDrawing && Vector2.Distance(pixelUV, prevUV) > 1)
                        {
                            for (float lerpT = 0; lerpT <= 1; lerpT += 0.01f)
                            {
                                Vector2 lerpV2 = Vector2.Lerp(prevUV, pixelUV, lerpT);
                                paintTarget.DrawTexture(lerpV2.x, lerpV2.y, brushSize, CopiedBrushTexture);
                            }
                        }

                        paintTarget.DrawTexture(pixelUV.x, pixelUV.y, brushSize, CopiedBrushTexture);

                        sameUvPoint = hit.textureCoord;
                        isDrawing = true;
                    }
                }
                else
                {
                    isDrawing = false;
                }
            }
            else
            {
                isDrawing = false;
            }

            print(isDrawing);
        }
        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .
        /// <summary> 브러시 색상 변경 </summary>
        public void SetBrushColor(in Color color)
        {
            brushColor = color;
            CopyBrushTexture();
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .

        /// <summary> 기본 형태(원)의 브러시 텍스쳐 생성 </summary>
        private void CreateDefaultBrushTexture()
        {
            int res = 512;
            float hRes = res * 0.5f;
            float sqrSize = hRes * hRes;

            brushTexture = new Texture2D(res, res);
            brushTexture.filterMode = FilterMode.Point;
            //brushTexture.alphaIsTransparency = true;
            defaultTexture = brushTexture;

            for (int y = 0; y < res; y++)
            {
                for (int x = 0; x < res; x++)
                {
                    // Sqaure Length From Center
                    float sqrLen = (hRes - x) * (hRes - x) + (hRes - y) * (hRes - y);
                    float alpha = Mathf.Max(sqrSize - sqrLen, 0f) / sqrSize;

                    //brushTexture.SetPixel(x, y, (sqrLen < sqrSize ? brushColor : Color.clear));
                    brushTexture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            brushTexture.Apply();
        }

        /// <summary> 원본 브러시 텍스쳐 -> 실제 브러시 텍스쳐(색상 적용) 복제 </summary>
        private void CopyBrushTexture()
        {
            if (brushTexture == null) return;

            // 기존의 카피 텍스쳐는 메모리 해제
            DestroyImmediate(CopiedBrushTexture);

            // 새롭게 할당
            {
                CopiedBrushTexture = new Texture2D(brushTexture.width, brushTexture.height);
                CopiedBrushTexture.filterMode = FilterMode.Point;
               // CopiedBrushTexture.alphaIsTransparency = true;
            }

            int height = brushTexture.height;
            int width = brushTexture.width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = brushColor;
                    c.a *= brushTexture.GetPixel(x, y).a;

                    CopiedBrushTexture.SetPixel(x, y, c);
                }
            }

            CopiedBrushTexture.Apply();

            Debug.Log("Copy Brush Texture");
        }

        #endregion
        /***********************************************************************
        *                               Editor Only
        ***********************************************************************/
        #region .
#if UNITY_EDITOR
        // 색상 변경 감지하여 브러시 텍스쳐 다시 복제
        private Color prevBrushColor;
        private float brushTextureUpdateCounter = 0f;
        private const float BrushTextureUpdateCounterInitValue = 0.7f;
        private void OnValidate()
        {
            if (Application.isPlaying && prevBrushColor != brushColor)
            {
                brushTextureUpdateCounter = BrushTextureUpdateCounterInitValue;
                prevBrushColor = brushColor;
            }
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
#endif
        private void UpdateBrushColorOnEditor()
        {
#if UNITY_EDITOR
            if (brushTextureUpdateCounter > 0f && 
                brushTextureUpdateCounter <= BrushTextureUpdateCounterInitValue)
            {
                brushTextureUpdateCounter -= Time.deltaTime;
            }

            if(brushTextureUpdateCounter < 0f)
            {
                CopyBrushTexture();
                brushTextureUpdateCounter = 9999f;
            }
#endif
        }
#endregion


        private void OnChangeColor(Color co)
        {
            print("change color");
            if (brushTexture != defaultTexture)
            {
                brushTexture = defaultTexture;
                CopyBrushTexture();
            }
            SetBrushColor(co);
        }


        private void OnClickBrushBtn()
        {
            brushTexture = defaultTexture;
            CopyBrushTexture();
            SetBrushColor(fcp.GetColor());
        }


        private void OnClickEraseBtn()
        {
            brushTexture = eraseTexture;
            CopyBrushTexture();
            SetBrushColor(Color.white);
        }

        public void OnBrushWidthScollbarValueChanged(float width)
        {
            if(width == 0)
            {
                brushSize = 0.01f;
            }
            else
            {
                brushSize = width * 0.3f; 
            }
        }
    }
}