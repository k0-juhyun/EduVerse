using UnityEngine;
using UnityEngine.EventSystems;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
#if UNITY_WEBGL
    public class PDFViewerPage : UIBehaviour
    {
		[SerializeField]
        private Texture2D m_HandCursor;

		public void ClearCache() {}
    }
#endif

#if !UNITY_WEBGL
    public class PDFViewerPage : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
        , IPointerEnterHandler, IPointerExitHandler
#endif
    {
		// 손 모양 커서 이미지
        [SerializeField]
        private Texture2D m_HandCursor;

		// PDF 뷰어 참조
        private PDFViewer m_Viewer;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
		// 포인터가 내부에 있는지 여부
        private bool m_PointerInside;
		// 손 모양 커서가 현재 설정되어있는지 여부
        private bool m_HandCursorSettedByMe;
#endif
		// 캔버스 카메라 캐시 여부
        private bool m_CanvasCameraCached;
		
		// 캔버스 카메라
        private Camera m_CanvasCamera;

		// 페이지 인덱스
        private int? m_PageIndex;

		// PDF 페이지
        private PDFPage m_Page;

		// 페이지 인덱스 프로퍼티
        public int PageIndex
        {
	        get
	        {
		        if (!m_PageIndex.HasValue)
			        m_PageIndex = transform.GetSiblingIndex();

		        return m_PageIndex.Value;
	        }
        }
        
		// 캐시 초기화 함수
        public void ClearCache()
        {
	        m_PageIndex = null;

            if (m_Page != null)
            {
                m_Page.Dispose();
                m_Page = null;
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
			// 포인터 클릭 시 실행
#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
	        if (!m_PointerInside)
		        return;
#endif

			// 뷰어 없으면 가져옴
            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();

			// 뷰어, 문서, 링크 액션 핸들러 없으면 종료
            if (m_Viewer == null || m_Viewer.Document == null || m_Viewer.LinksActionHandler == null)
	            return;

            PDFPage page = m_Page;
			
			bool unloadPage = false;
			
			if (page == null)
			{
				page = m_Viewer.Document.GetPage(PageIndex);
				
				unloadPage = true;
			}
			
			// 페이지가 없으면 현재 페이지를 가져오고 나중에 
			try
			{

				PDFLink link = GetLinkAtPoint(page, eventData.pressPosition, eventData.pressEventCamera);

				if (link != null)
				{
					PDFActionHandlerHelper.ExecuteLinkAction(link, m_Viewer);
				}
				else if (m_Viewer.ParagraphZoomingEnable && eventData.clickCount == 2)
				{
					using (PDFTextPage textPage = page.GetTextPage())
					{
						Vector2 pos = eventData.pressPosition;
						RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, pos, GetComponent<Camera>(), out pos);
						RectTransform rt = (RectTransform)transform;
						pos += rt.sizeDelta.x * 0.5f * Vector2.right;

						pos.y = -pos.y + rt.sizeDelta.y * 0.5f;

						pos = pos.x * (rt.sizeDelta.y / rt.sizeDelta.x) * Vector2.right + pos.y * Vector2.up;

						Vector2 pagePoint = page.DeviceToPage(0, 0, (int)rt.sizeDelta.y, (int)rt.sizeDelta.y, PDFPage.PageRotation.Normal, (int)pos.x, (int)pos.y);
						Vector2 pageSize = page.GetPageSize();

						float threshold = m_Viewer.ParagraphDetectionThreshold;

						string text = GetBoundedText(textPage, 0.0f, pagePoint.y + 0.0f, pageSize.x, pagePoint.y - 1.0f);

						if (!string.IsNullOrEmpty(text.Trim()) && text.Trim().Length > 4)
						{
							string prevText = text;

							float bottomOffset = 0.0f;
							float topOffset = 0.0f;
							float t = 0.0f;

							while (true)
							{
								bottomOffset += 2.0f;
								text = GetBoundedText(textPage, 0.0f, pagePoint.y + 0.0f, pageSize.x, pagePoint.y - bottomOffset);

								if (text == prevText)
									t += 2.0f;
								else
									t = 0.0f;
								if (t >= threshold)
									break;

								prevText = text;
							}

							t = 0.0f;
							while (true)
							{
								topOffset += 2.0f;
								text = GetBoundedText(textPage, 0.0f, pagePoint.y + topOffset, pageSize.x, pagePoint.y - bottomOffset);

								if (text == prevText)
									t += 2.0f;
								else
									t = 0.0f;
								if (t >= threshold)
									break;

								prevText = text;
							}

							Rect pageRect = new Rect(0.0f, pagePoint.y + topOffset, pageSize.x, (pagePoint.y + topOffset) - (pagePoint.y - bottomOffset));

							m_Viewer.ZoomOnParagraph(this, pageRect);
						}
					}
				}
			}
			finally
			{
				if (unloadPage)
				{
					page.Dispose();
				}
			}
        }

        private string GetBoundedText(PDFTextPage textPage, float left, float top, float right, float bottom)
        {
            return textPage.GetBoundedText(left, top, right, bottom, 4096);
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (m_Page == null)
                m_Page = m_Viewer.Document.GetPage(PageIndex);

            m_PointerInside = true;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (m_Page != null)
            {
                m_Page.Dispose();
                m_Page = null;
            }

            m_PointerInside = false;

            if (m_HandCursorSettedByMe)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
#endif
        protected override void OnEnable()
        {
            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();

            if (!m_CanvasCameraCached)
			{
                m_CanvasCamera = FindCanvasCamera((RectTransform)transform);

                m_CanvasCameraCached = true;
            }
                
            if (m_Page != null)
            {
                m_Page.Dispose();
                m_Page = null;
            }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
            m_PointerInside = false;
#endif
        }

        private Camera FindCanvasCamera(RectTransform rt)
        {
	        RectTransform parent = (RectTransform)rt.parent;
            
	        if (parent != null)
            {
                Canvas canvas = parent.GetComponent<Canvas>();

                if (canvas != null)
                {
	                if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
	                {
		                return canvas.worldCamera;
                    }

	                return null;
                }

                return FindCanvasCamera(parent);
            }

            return null;
        }

        protected override void OnTransformParentChanged()
		{
            base.OnTransformParentChanged();

            m_CanvasCameraCached = false;
            m_CanvasCamera = null;
		}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN|| UNITY_STANDALONE_OSX
        private void Update()
        {
            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();

            if (m_HandCursor == null || !m_Viewer.ChangeCursorWhenOverURL)
                return;

            if (!m_CanvasCameraCached)
            {
                m_CanvasCamera = FindCanvasCamera((RectTransform)transform);

                m_CanvasCameraCached = true;
            }

            if (m_PointerInside)
			{
				Vector2 pointerPosition = Input.mousePosition;

				PDFLink link = GetLinkAtPoint(m_Page, pointerPosition, m_CanvasCamera);

				if (link != null)
				{
					Cursor.SetCursor(m_HandCursor, new Vector2(6.0f, 0.0f), CursorMode.Auto);
					m_HandCursorSettedByMe = true;
				}
				else
				{
					Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
					m_HandCursorSettedByMe = false;
				}
			}
		}
#endif
		
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) { }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) { }

        private PDFLink GetLinkAtPoint(PDFPage page, Vector2 point, Camera camera)
        {
            if (m_Viewer == null)
                m_Viewer = GetComponentInParent<PDFViewer>();

	        Vector2 localPointerPosition;
	        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, point, camera,
		        out localPointerPosition))
	        {
		        RectTransform rt = (RectTransform)transform;

		        localPointerPosition += rt.sizeDelta.x * 0.5f * Vector2.right;

		        localPointerPosition.y = -localPointerPosition.y + rt.sizeDelta.y * 0.5f;

		        localPointerPosition = localPointerPosition.x * (rt.sizeDelta.y / rt.sizeDelta.x) * Vector2.right +
		                                localPointerPosition.y * Vector2.up;

		        Vector2 pagePoint = page.DeviceToPage(0, 0, (int)rt.sizeDelta.y, (int)rt.sizeDelta.y,
			        PDFPage.PageRotation.Normal,
			        (int)localPointerPosition.x, (int)localPointerPosition.y);

		        return page.GetLinkAtPoint(pagePoint);
	        }

	        return null;
        }
    }
#endif
}