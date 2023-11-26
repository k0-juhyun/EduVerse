using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Paroxe.PdfRenderer.Internal;
using Paroxe.PdfRenderer.Internal.Viewer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Paroxe.PdfRenderer.WebGL;
using System.IO;

namespace Paroxe.PdfRenderer
{
    /// <summary>
    /// PDFViewer is an Unity UI component that allow you to visualize PDF Document.
    /// </summary>
    public class PDFViewer : UIBehaviour, IPDFDevice, IPDFColoredRectListProvider
    {
        #region 변수
        // PDFViewerInternal pdf 뷰어의 내부 기능을 다룸
        [SerializeField]
        public PDFViewerInternal m_Internal;

        // 책갈피와 관련된 동작을 처리하는 인터페이스 변수
        private IPDFDeviceActionHandler m_BookmarksActionHandler;

        // 링크와 관련된 동작을 처리하는 인터페이스 변수
        private IPDFDeviceActionHandler m_LinksActionHandler;

        // 현재 보이는 페이지 범위를 나타내는 인스턴스
        private PDFPageRange m_CurrentPageRange;

        // 검색 결과를 나타내는 인스턴스
        private PDFSearchResult m_CurrentSearchResult;

        // PDF 문서를 나타내는 인스턴스
        private PDFDocument m_Document;

        // warning 무시한다는 의미
        // 경고를 무시하고 사용되지 않는 변수, PDF클래스의 인스턴스
#pragma warning disable 414
        private PDFDocument m_SuppliedDocument;

        // PDF 페이지의 텍스처를 보관하는  클래스 배열
#pragma warning restore 414
        private PDFPageTextureHolder[] m_PageTextureHolders;

        // 현재 검색 결과의 인덱스
        private int m_CurrentSearchResultIndex;

        // 현재 검색 결과의 인덱스
        private int m_CurrentSearchResultIndexWithinCurrentPage;

        // 현재 페이지내에서의 검색 결과의 인덱스
        private bool m_DelayedOnEnable;

        // 잘못된 비밀번호 메시지가 표시되는 지연시간.
        private float m_InvalidPasswordMessageDelay;

        // WebGL이 아니거나 에디터에서 실행 중인 경우에만 실행
#if !UNITY_WEBGL || UNITY_EDITOR
        // 비밀번호 오류 메시지가 사라지기 전의 지연시간.
        private float m_InvalidPasswordMessageDelayBeforeFade = 0.5f;

        // 다운로드가 취소되었는지 여부
        private bool m_DownloadCanceled = false;
#endif
        // 잘못된 비밀번호 메시지가 보이는지 여부
        private bool m_InvalidPasswordMessageVisisble;

        // 문서가 로드되었는지 여부를 나타내는 변수
        private bool m_IsLoaded;

        // 로드할 페이지의 인덱스
        private int m_LoadAtPageIndex;

        // 오버레이의 투명도를 나타내는 값
        private float OverlayAlpha = 0.50f;

        // 오버레이가 보이는지 여부를 나타내는 변수
        private bool OverlayVisible;

        // 전체 페이지 수
        private int m_PageCount;

        // 페이지의 오프셋 값 배열
        private float[] m_PageOffsets;

        // 페이지의 크기 배열
        private Vector2[] m_PageSizes;

        // 정규화된 페이지 크기 배열
        private Vector2[] m_NormalPageSizes;

        // 보류 중인 문서 버퍼
        private byte[] m_PendingDocumentBuffer;

        // 이전에 보이는 페이지의 인덱스
        private int m_PreviousMostVisiblePage = -1;

        // 이전 페이지 맞춤
        private PageFittingType m_PreviousPageFitting;

        // 이전 줌 팩터 값
        private float m_PreviousZoom;

        //이전 줌 팩터로 이동할 값
        private float m_PreviousZoomToGo;

        // 검색 결과를 보관하는 배열
        private IList<PDFSearchResult>[] m_SearchResults;

        // 시작 줌
        private float m_StartZoom;

        // 업데이트 변경 지연
        private float m_UpdateChangeDelay;

        // 줌 위치
        private Vector2 m_ZoomPosition = Vector2.zero;

        // PDF렌더러 인스턴스
        private PDFRenderer m_Renderer;

        // 이전 터치 수
        private int m_PreviousTouchCount;

        // 확대 시작 값
        private float m_PinchZoomStartZoomFactor;

        // 확대 시작 시의 변위 값
        private float m_PinchZoomStartDeltaMag;

        // 캔버스
        private Canvas m_Canvas;

        // 캔버스에 들어가는 그래픽 레이캐스트
        private GraphicRaycaster m_GraphicRaycaster;

        // 캔버스 리스트
        private List<Canvas> m_CanvasList = new List<Canvas>();

        // 뷰포트 스크롤
        private ScrollRect m_ViewportScrollRect;

        // 수평 스크롤바
        private Scrollbar m_HorizontalScrollBar;

        // 수직 스크롤 바
        private Scrollbar m_VerticalScrollBar;

        // 마지막으로 설정된 레이블 페이지의 인덱스
        private int? m_LastSetLabelPageIndex;

        // 마지막으로 설정된 레이블 페이지 수
        private int? m_LastSetLabelPageCount;

        // PDF 썸네일 뷰어
        private PDFThumbnailsViewer m_ThumbnailsViewer;

        // PDF 북마크 뷰어
        private PDFBookmarksViewer m_BookmarksViewer;

        // URL 열 수 있는지 여부 설정
        [SerializeField]
        private bool m_AllowOpenURL = true;

        // URL 위에 마우스 커서를 놓았을 때 커서 변경 여부
        [SerializeField]
        private bool m_ChangeCursorWhenOverURL = true;

        // 바이트 공급 오브젝트 설정, PDF 문서의 내용을 제공한다.
        [SerializeField]
        private GameObject m_BytesSupplierObject;

        // // 바이트 공급 컴포넌트 설정,
        [SerializeField]
        private Component m_BytesSupplierComponent;

        // 바이트 공급 함수의 이름 설정
        [SerializeField]
        private string m_BytesSupplierFunctionName;

        // PDF 파일의 이름을 설정
        [SerializeField]
        private string m_FileName = "";

        // PDF 파일의 경로를 설정
        [SerializeField]
        private string m_FilePath = "";

        // PDF 파일의 소스 유형을 설정
        [SerializeField]
        private FileSourceType m_FileSource = FileSourceType.Resources;

        // PDF 파일의 URL 설정
        [SerializeField]
        private string m_FileURL = "";

        // PDF 파일이 있는 폴더의 경로 설정
        [SerializeField]
        private string m_Folder = "";

        // 활성화될 떄 PDF 파일을 로드할지 여부 설정.
        [SerializeField]
        private bool m_LoadOnEnable = true;

        // 최대 확대 설정
        [SerializeField]
        private float m_MaxZoomFactor = 8.0f;

        // 최대 확대 상태에서의 텍스처 품질 설정.
        [SerializeField]
        private float m_MaxZoomFactorTextureQuality = 4.0f;

        // 최소 축소 설정
        [SerializeField]
        private float m_MinZoomFactor = 0.25f;

        // 페이지 맞춤 유형 설정
        [SerializeField]
        private PageFittingType m_PageFitting = PageFittingType.Zoom;

        // PDF파일의 암호 설정
        [SerializeField]
        private string m_Password = "";

        // PDF Asset 오브젝트
        [SerializeField]
        private PDFAsset m_PDFAsset = null;

        // 초기 확대 값
        [SerializeField]
        private float m_ZoomFactor = 1.0f;

        // 확대 단계를 설정
        [SerializeField]
        private float m_ZoomStep = 0.25f;

        // 확대 설정
        [SerializeField]
        private float m_ZoomToGo;

        // 페이지 사이의 수직 여백 설정
        [SerializeField]
        private float m_VerticalMarginBetweenPages = 200;

        // 비활성화 될 때 PDF 파일을 언로드할지 여부를 설정.
        [SerializeField]
        private bool m_UnloadOnDisable;

        // 수직 스크롤바 표시여부 설정
        [SerializeField]
        private bool m_ShowVerticalScrollBar = false;

        // 북마크 뷰어를 표시할지 여부 설정
        [SerializeField]
        private bool m_ShowBookmarksViewer = true;

        // 수평 스크롤바 표시할 지 여부 설정
        [SerializeField]
        private bool m_ShowHorizontalScrollBar = true;

        // 썸네일 뷰어를 표시할지 여부 설정
        [SerializeField]
        private bool m_ShowThumbnailsViewer = true;

        // 상단 바를 표시할지 여부 설정
        [SerializeField]
        private bool m_ShowTopBar = false;

        // 스크롤 감도 설정
        [SerializeField]
        private float m_ScrollSensitivity = 75.0f;

        // 검색 결과의 색상을 설정
        [SerializeField]
        private Color m_SearchResultColor = new Color(0.0f, 115 / 255.0f, 230 / 255.0f, 125 / 255.0f);

        // 검색 결과 주변의 여백 설정
        [SerializeField]
        private Vector2 m_SearchResultPadding = new Vector2(2.0f, 4.0f);

        // 프레임당 할단된 검색 시간을 설정
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_SearchTimeBudgetPerFrame = 0.60f;

        // PDF 렌더링 설정
        [SerializeField]
        private PDFRenderer.RenderSettings m_RenderSettings = new PDFRenderer.RenderSettings();

        // 줌 후 업데이트 전 지연 시간을 설정.
        [SerializeField]
        private float m_DelayAfterZoomingBeforeUpdate = 0.005f;

        // 단락 줌 팩터를 설정
        [SerializeField]
        private float m_ParagraphZoomFactor = 2.0f;

        // 단락 줌 활성화 여부를 설정.
        [SerializeField]
        private bool m_ParagraphZoomingEnable = true;

        // 단락을 감지하는데 사용하는 임계값
        [SerializeField]
        private float m_ParagraphDetectionThreshold = 12.0f;     


        #endregion

        #region 델리게이트 & 이벤트
        // 델리게이트 선언 : 각종 이벤트 핸들러들을 정의
        public delegate void CancelEventHandler(PDFViewer sender);
        public delegate void CurrentPageChangedEventHandler(PDFViewer sender, int oldPageIndex, int newPageIndex);
        public delegate void DocumentChangedEventHandler(PDFViewer sender, PDFDocument document);
        public delegate void LoadFailEventHandler(PDFViewer sender);
        public delegate void PDFViewerEventHandler(PDFViewer sender);
        public delegate void ZoomChangedEventHandler(PDFViewer sender, float oldZoom, float newZoom);

        // 이벤트들 : 위에서 정의한 델리게이트를 이용하여 이벤트 선언
        public event CurrentPageChangedEventHandler OnCurrentPageChanged;
        public event PDFViewerEventHandler OnDisabled;
        public event DocumentChangedEventHandler OnDocumentLoaded;
        public event LoadFailEventHandler OnDocumentLoadFailed;
        public event DocumentChangedEventHandler OnDocumentUnloaded;
        public event CancelEventHandler OnDownloadCancelled;
        public event CancelEventHandler OnPasswordCancelled;
        public event ZoomChangedEventHandler OnZoomChanged;
        #endregion

        #region 프로퍼티
        // 파일 소스 타입 열거형 
        // 파일 소스의 종류를 정의함.

        public enum FileSourceType
        {
            None,
            Web,
            StreamingAssets,
            Resources,
            FilePath,
            Bytes,
            Asset,
            DocumentObject,
            PersistentData
        }

        // 페이지 피팅 타입 열거형 : 페이지가 뷰어에 맞게 표시되는 방식을 정의
        public enum PageFittingType
        {
            ViewerWidth,
            ViewerHeight,
            WholePage,
            Zoom
        }

        // 뷰어 모드 타입 열거형 : 뷰어의 동작 모드를 정의함.
        public enum ViewerModeType
        {
            Move,
            ZoomOut,
            ZoomIn
        }

        /// <summary>
        /// 부모 캔버스 반환
        /// </summary>
        public Canvas canvas
        {
            get
            {
                if (m_Canvas == null)
                    CacheCanvas();
                return m_Canvas;
            }
        }

        /// <summary>
        /// 외부 브라우저에서 URL 링크를 열 수 있는지 체크하는 프로퍼티
        /// </summary>
        public bool AllowOpenURL
        {
            get { return m_AllowOpenURL; }
            set { m_AllowOpenURL = value; }
        }

        /// <summary>
        /// ChangeCursorWhenOverURL 프로퍼티: 
        /// URL 링크 위에 마우스 커서가 올라갔을 때 
        /// 커서를 변경할지 여부를 설정하거나 반환합니다.
        /// </summary>
        public bool ChangeCursorWhenOverURL
        {
            get { return m_ChangeCursorWhenOverURL; }
            set { m_ChangeCursorWhenOverURL = value; }
        }

        /// <summary>
        /// 뷰포트의 배경색을 설정하거나 반환합니다.
        /// </summary>
        public Color BackgroundColor
        {
            get { return m_Internal.Viewport.GetComponent<Image>().color; }
            set
            {
                if (m_Internal.Viewport.GetComponent<Image>().color != value)
                    m_Internal.Viewport.GetComponent<Image>().color = value;
            }
        }

        /// <summary>
        /// 프레임당 텍스트 검색에 할당되는 시간의 상대적인 양을 설정하거나 반환합니다.
        /// </summary>
        public float SearchTimeBudgetPerFrame
        {
            get { return m_SearchTimeBudgetPerFrame; }
            set { m_SearchTimeBudgetPerFrame = Mathf.Clamp01(value); }
        }

        /// <summary>
        /// 북마크와 링크에 대한 액션 핸들러를 설정하거나 반환합니다.
        /// </summary>
        public IPDFDeviceActionHandler BookmarksActionHandler
        {
            get { return m_BookmarksActionHandler; }
            set { m_BookmarksActionHandler = value; }
        }
        public IPDFDeviceActionHandler LinksActionHandler
        {
            get { return m_LinksActionHandler; }
            set { m_LinksActionHandler = value; }
        }

        /// <summary>
        /// 파일 소스가 바이트 배열일 때 해당하는 컴포넌트와 함수 이름을 설정하거나
        /// 반환합니다.
        /// </summary>
        public Component BytesSupplierComponent
        {
            get { return m_BytesSupplierComponent; }
            set { m_BytesSupplierComponent = value; }
        }
        public string BytesSupplierFunctionName
        {
            get { return m_BytesSupplierFunctionName; }
            set { m_BytesSupplierFunctionName = value; }
        }

        // 현재 가장 많이 보이는 페이지의 인덱스를 가져오거나 설정.
        public int CurrentPageIndex
        {
            get { return GetMostVisiblePageIndex(); }
            set
            {
                int mostVisible = GetMostVisiblePageIndex();

                if (value != mostVisible)
                    GoToPage(value);
            }
        }

        // 현재 검색 결과의 인덱스를 반환
        public int CurrentSearchResultIndex
        {
            get { return m_CurrentSearchResultIndex; }
        }

        /// <summary>
        ///  현재 로드된 PDF 문서의 바이트 배열을 반환합니다.
        /// </summary>
        public byte[] DataBuffer
        {
            get
            {
                if (Document != null)
                    return m_Document.DocumentBuffer;
                return null;
            }
        }

        // 현재 로드된 PDF 문서를 반환
        public PDFDocument Document
        {
            get { return m_Document; }
        }

        // PDF 파일의 이름을 가져오거나 설정
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value != null ? value.Trim() : ""; }
        }

        // PDF 파일의 경로를 가져오거나 설정
        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }


        // PDF 파일의 소스 타입을 가져오거나 설정
        public FileSourceType FileSource
        {
            get { return m_FileSource; }

            set { m_FileSource = value; }
        }


        // 웹에서 PDF 파일을 로드할 떄 사용되는 URL을 가져오거나 설정
        public string FileURL
        {
            get { return m_FileURL; }
            set { m_FileURL = value; }
        }

        // PDF 파일이 위치한 폴더를 가져오거나 설정.
        public string Folder
        {
            get { return m_Folder; }
            set { m_Folder = value; }
        }

        // PDF 문서가 로드되었는지 여부를 반환.
        public bool IsLoaded
        {
            get { return m_IsLoaded; }
        }

        // 활성화 될 때 PDF 문서를 자동으로 로드할지 여부를 가져오거나 설정.
        public bool LoadOnEnable
        {
            get { return m_LoadOnEnable; }
            set { m_LoadOnEnable = value; }
        }

        // 텍스쳐 품질을 기준으로 한 최대 줌 팩터를 가져오거나 설정.
        public float MaxZoomFactorTextureQuality
        {
            get { return m_MaxZoomFactorTextureQuality; }
            set
            {
                if (Math.Abs(Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor) - m_MaxZoomFactorTextureQuality) > float.Epsilon)
                {
                    m_MaxZoomFactorTextureQuality = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);

                    m_UpdateChangeDelay = 1.0f;
                }
            }
        }

        // 최소 줌 팩터
        public float MinZoomFactor
        {
            get { return m_MinZoomFactor; }
            set
            {
                m_MinZoomFactor = value;

                if (m_MinZoomFactor < 0.01f)
                    m_MinZoomFactor = 0.01f;
            }
        }

        // 최대 줌 팩터
        public float MaxZoomFactor
        {
            get { return m_MaxZoomFactor; }
            set
            {
                m_MaxZoomFactor = value;

                if (m_MaxZoomFactor < m_MinZoomFactor)
                    m_MaxZoomFactor = m_MinZoomFactor;
            }
        }

        // 페이지 피팅 타입을 가져오거나 설정
        public PageFittingType PageFitting
        {
            get { return m_PageFitting; }
            set { m_PageFitting = value; }
        }

        // PDF 문서의 암호
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        ///  PDF Asset 프로퍼티
        /// </summary>
        public PDFAsset PDFAsset
        {
            get { return m_PDFAsset; }
            set { m_PDFAsset = value; }
        }

        // 주석을 렌더링 할지 여부
        public bool RenderAnnotations
        {
            get { return m_RenderSettings.renderAnnotations; }

            set
            {
                if (m_RenderSettings.renderAnnotations != value)
                {
                    m_RenderSettings.renderAnnotations = value;

                    m_UpdateChangeDelay = 0.1f;
                }
            }
        }


        // 그레이 스케일로 설정할지 체크
        public bool RenderGrayscale
        {
            get { return m_RenderSettings.grayscale; }

            set
            {
                if (m_RenderSettings.grayscale != value)
                {
                    m_RenderSettings.grayscale = value;

                    m_UpdateChangeDelay = 0.1f;
                }
            }
        }

        // 스크롤 감도를 가져오거나 설정합니다.
        public float ScrollSensitivity
        {
            get { return m_ScrollSensitivity; }
            set { m_ScrollSensitivity = value; }
        }

        // 검색 결과의 색상 프로퍼티
        public Color SearchResultColor
        {
            get { return m_SearchResultColor; }
            set
            {
                if (m_SearchResultColor != value)
                {
                    m_SearchResultColor = value;
                    m_UpdateChangeDelay = 0.25f;
                }
            }
        }


        // 검색 결과의 패딩
        public Vector2 SearchResultPadding
        {
            get { return m_SearchResultPadding; }
            set
            {
                if (m_SearchResultPadding != value)
                {
                    m_SearchResultPadding = value;
                    m_UpdateChangeDelay = 0.25f;
                }
            }
        }

        // 북마크 표시 여부 프로퍼티
        public bool ShowBookmarksViewer
        {
            get { return m_ShowBookmarksViewer; }
            set
            {
                if (m_ShowBookmarksViewer != value)
                {
                    m_ShowBookmarksViewer = value;

                    UpdateBookmarksViewerVisibility(m_ShowBookmarksViewer);
                }
            }
        }

        // 북마크 뷰어의 가시성을 업데이트
        private void UpdateBookmarksViewerVisibility(bool visible)
        {
            if (visible && m_IsLoaded)
            {
#if !UNITY_WEBGL
                if (m_BookmarksViewer.RootBookmark == null || m_BookmarksViewer.RootBookmark.ChildCount == 0)
#endif
                    visible = false;
            }

            if (m_Internal.LeftPanel != null)
            {
                m_Internal.LeftPanel.Bookmarks.gameObject.SetActive(visible);
                m_Internal.LeftPanel.BookmarksTab.gameObject.SetActive(visible);

                m_Internal.LeftPanel.SetActive(m_ShowThumbnailsViewer || visible);

                if (!visible && m_ShowThumbnailsViewer)
                    m_Internal.LeftPanel.OnThumbnailsTabClicked();
                else if (visible && !m_ShowThumbnailsViewer)
                    m_Internal.LeftPanel.OnBookmarksTabClicked();
                else
                    m_Internal.LeftPanel.OnBookmarksTabClicked();
            }
        }

        // 가로 스크롤바 표시 프로퍼티
        public bool ShowHorizontalScrollBar
        {
            get { return m_ShowHorizontalScrollBar; }
            set
            {
                if (m_ShowHorizontalScrollBar != value)
                {
                    m_ShowHorizontalScrollBar = value;

                    UpdateScrollBarVisibility();
                }

            }
        }

        // 썸네일 뷰어 표시 프로퍼티
        public bool ShowThumbnailsViewer
        {
            get { return m_ShowThumbnailsViewer; }
            set
            {
                if (m_ShowThumbnailsViewer != value)
                {
                    m_ShowThumbnailsViewer = value;

                    if (m_Internal.LeftPanel != null)
                    {
                        m_Internal.LeftPanel.ThumbnailsViewer.gameObject.SetActive(m_ShowThumbnailsViewer);
                        m_Internal.LeftPanel.ThumbnailsTab.gameObject.SetActive(m_ShowThumbnailsViewer);

                        m_Internal.LeftPanel.SetActive(m_ShowThumbnailsViewer || m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf);

                        if (!m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf && m_ShowThumbnailsViewer)
                            m_Internal.LeftPanel.OnThumbnailsTabClicked();
                        else if (m_Internal.LeftPanel.Bookmarks.gameObject.activeSelf && !m_ShowThumbnailsViewer)
                            m_Internal.LeftPanel.OnBookmarksTabClicked();
                        else
                            m_Internal.LeftPanel.OnBookmarksTabClicked();
                    }
                }
            }
        }


        // 상단 바 표시 프로퍼티
        public bool ShowTopBar
        {
            get { return m_ShowTopBar; }
            set
            {
                if (m_ShowTopBar != value)
                {
                    m_ShowTopBar = value;

                    if (!m_ShowTopBar)
                    {
                        m_Internal.TopPanel.gameObject.SetActive(false);
                        m_Internal.TopPanel.sizeDelta = new Vector2(0.0f, 0.0f);

                        m_Internal.Viewport.offsetMax = new Vector2(m_Internal.Viewport.offsetMax.x, 0.0f);
                        m_Internal.VerticalScrollBar.offsetMax =
                            new Vector2(m_Internal.VerticalScrollBar.offsetMax.x, 0.0f);

                        if (m_Internal.LeftPanel != null)
                        {
                            ((RectTransform)m_Internal.LeftPanel.transform).sizeDelta =
                                new Vector2(((RectTransform)m_Internal.LeftPanel.transform).sizeDelta.x, 0.0f);
                        }
                    }
                    else
                    {
                        m_Internal.TopPanel.gameObject.SetActive(true);
                        m_Internal.TopPanel.sizeDelta = new Vector2(0.0f, 60.0f);

                        m_Internal.Viewport.offsetMax = new Vector2(m_Internal.Viewport.offsetMax.x, -60.0f);
                        m_Internal.VerticalScrollBar.offsetMax =
                            new Vector2(m_Internal.VerticalScrollBar.offsetMax.x, -59.0f);

                        if (m_Internal.LeftPanel != null)
                        {
                            ((RectTransform)m_Internal.LeftPanel.transform).sizeDelta =
                                new Vector2(((RectTransform)m_Internal.LeftPanel.transform).sizeDelta.x, -59.0f);
                        }
                    }
                }
            }
        }


        // 세로 스크롤바 프로퍼티
        public bool ShowVerticalScrollBar
        {
            get { return m_ShowVerticalScrollBar; }
            set
            {
                if (m_ShowVerticalScrollBar != value)
                {
                    m_ShowVerticalScrollBar = value;

                    UpdateScrollBarVisibility();
                }
            }
        }

        // 비활성화 될 때 PDF 문서를 자동으로 업로드 할지 여부
        public bool UnloadOnDisable
        {
            get { return m_UnloadOnDisable; }
            set { m_UnloadOnDisable = value; }
        }

        // 페이지 간의 수직 여백 프로퍼티
        public float VerticalMarginBetweenPages
        {
            get { return m_VerticalMarginBetweenPages; }
            set
            {
                if (m_VerticalMarginBetweenPages != value)
                {
                    if (value < 0.0f)
                    {
                        m_VerticalMarginBetweenPages = 0.0f;
                    }
                    else
                    {
                        m_VerticalMarginBetweenPages = value;
                    }

                    if (m_IsLoaded)
                    {
                        ComputePageOffsets();
                        UpdatePagesPlacement();
                        m_Internal.PageContainer.sizeDelta = GetDocumentSize();
                        EnsureValidPageContainerPosition();
                    }
                }
            }
        }

        // 현재 줌 팩터 프로퍼티
        public float ZoomFactor
        {
            get { return m_ZoomToGo; }
            set
            {
                if (Math.Abs(m_ZoomToGo - Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor)) > float.Epsilon)
                {
                    m_ZoomToGo = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);

                    m_ZoomPosition = new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f);

                    NotifyZoomChanged(m_PreviousZoomToGo, m_ZoomToGo);

                    m_PageFitting = PageFittingType.Zoom;
                }
            }
        }

        // 줌 단계 프로퍼티
        public float ZoomStep
        {
            get { return m_ZoomStep; }
            set { m_ZoomStep = value; }
        }

        // 문단 줌 기능 활성화 프로퍼티
        public bool ParagraphZoomingEnable
        {
            get { return m_ParagraphZoomingEnable; }
            set { m_ParagraphZoomingEnable = value; }
        }

        // 문단 줌 팩터 프로퍼티
        public float ParagraphZoomFactor
        {
            get { return m_ParagraphZoomFactor; }
            set { m_ParagraphZoomFactor = value; }
        }

        // 문단 감지 임계값 프로퍼티
        public float ParagraphDetectionThreshold
        {
            get { return m_ParagraphDetectionThreshold; }
            set { m_ParagraphDetectionThreshold = value; }
        }


        #endregion

        #region PDF문서로드
        // PDF 문서를 로드 LoadDocument 오버로드
        #region LoadDocument 오버로드
        public void LoadDocument(int pageIndex = 0)
        {
            if (m_IsLoaded)
                CleanUp();

            CommonLoad();
        }
        // PDF 문서 로드 LoadDocument 오버로드
        public void LoadDocument(PDFDocument document, int pageIndex = 0)
        {
            LoadDocument(document, null, pageIndex);
        }
        // PDF 문서 로드 LoadDocument 오버로드
        public void LoadDocument(PDFDocument document, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.DocumentObject;

            m_SuppliedDocument = document;
            m_Password = password;

            CommonLoad();
        }
        #endregion

        //  Asset에서 PDF 문서를 로드합니다. LoadDocumentFromAsset 오버로드
        #region LoadDocumentFromAsset 오버로드
        public void LoadDocumentFromAsset(PDFAsset pdfAsset, int pageIndex = 0)
        {
            LoadDocumentFromAsset(pdfAsset, null, pageIndex);
        }

        //  Asset에서 PDF 문서를 로드합니다. LoadDocumentFromAsset 오버로드
        public void LoadDocumentFromAsset(PDFAsset pdfAsset, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
            {
                CleanUp();
            }

            m_FileSource = FileSourceType.Asset;

            m_PDFAsset = pdfAsset;
            m_Password = password;

            CommonLoad(pdfAsset.m_FileContent);
        }
        #endregion

        // Resources에서 PDF 로드 LoadDocumentFromResources 오버로드
        #region LoadDocumentFromResources 오버로드
        public void LoadDocumentFromResources(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromResources(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromResources(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Resources;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }
        #endregion

        // StreamingAssets 에서 PDF 로드 LoadDocumentFromStreamingAssets  오버로드
        #region LoadDocumentFromStreamingAssets 오버로드
        public void LoadDocumentFromStreamingAssets(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromStreamingAssets(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromStreamingAssets(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.StreamingAssets;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }
        #endregion

        // PersistentData에서 PDF 로드 LoadDocumentFromPersistentData 오버로드
        #region LoadDocumentFromPersistentData 오버로드
        public void LoadDocumentFromPersistentData(string folder, string fileName, int pageIndex = 0)
        {
            LoadDocumentFromPersistentData(folder, fileName, null, pageIndex);
        }

        public void LoadDocumentFromPersistentData(string folder, string fileName, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.PersistentData;

            m_Folder = folder;
            m_FileName = fileName;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }
        #endregion

        //  웹에서 PDF 문서를 로드 LoadDocumentFromWeb 오버로드
        #region LoadDocumentFromWeb 오버로드
        public void LoadDocumentFromWeb(string url, int pageIndex = 0)
        {
            LoadDocumentFromWeb(url, null, pageIndex);
        }

        public void LoadDocumentFromWeb(string url, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Web;

            m_FileURL = url;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }

        #endregion

        // 버퍼에서 PDF문서 로드 LoadDocumentFromBuffer 오버로드
        #region LoadDocumentFromBuffer 오버로드
        public void LoadDocumentFromBuffer(byte[] buffer, int pageIndex = 0)
        {
            LoadDocumentFromBuffer(buffer, null, pageIndex);
        }

        public void LoadDocumentFromBuffer(byte[] buffer, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.Bytes;

            m_Password = password;

            CommonLoad(buffer);
        }
        #endregion

        // 파일에서 PDF문서 로드 LoadDocumentFromFile 오버로드
        #region LoadDocumentFromFile 오버로드
        public void LoadDocumentFromFile(string filePath, int pageIndex = 0)
        {
            LoadDocumentFromFile(filePath, null, pageIndex);
        }

        public void LoadDocumentFromFile(string filePath, string password, int pageIndex = 0)
        {
            m_LoadAtPageIndex = pageIndex;

            if (m_IsLoaded)
                CleanUp();

            m_FileSource = FileSourceType.FilePath;

            m_FilePath = filePath;
            m_Password = password;
            m_FilePath = GetFileLocation();

            m_Password = password;

            CommonLoad();
        }
        #endregion
        #endregion


        // PageFittingType에 따라 줌 팩터 조정
        public void AdjustZoomToPageFitting(PageFittingType pageFitting, Vector2 referencePageSize)
        {
            switch (pageFitting)
            {
                case PageFittingType.ViewerWidth:
                    {
                        float firstPageWidth = referencePageSize.x;
                        float viewportWidth = m_Internal.Viewport.rect.size.x;
                        m_ZoomToGo = viewportWidth / (firstPageWidth*2) ;

                        break;
                    }
                case PageFittingType.ViewerHeight:
                    {
                        float firstPageHeight = referencePageSize.y;
                        float viewportHeight = m_Internal.Viewport.rect.size.y;
                        m_ZoomToGo = viewportHeight / firstPageHeight;

                        break;
                    }
                case PageFittingType.WholePage:
                    {
                        float firstPageWidth = referencePageSize.x;
                        float firstPageHeight = referencePageSize.y + 2.0f * m_VerticalMarginBetweenPages;
                        float viewportWidth = m_Internal.Viewport.rect.size.x;
                        float viewportHeight = m_Internal.Viewport.rect.size.y;

                        m_ZoomToGo = Mathf.Min(viewportWidth / firstPageWidth, viewportHeight / firstPageHeight);

                        break;
                    }
                case PageFittingType.Zoom:
                    {
                        break;
                    }
            }

            // 수정 코드
            //switch (pageFitting)
            //{
            //    case PageFittingType.ViewerWidth:
            //        {
            //            float firstPageWidth = referencePageSize.x;
            //            float viewportWidth = m_Internal.Viewport.rect.size.x;
            //            m_ZoomToGo = viewportWidth / (firstPageWidth * 4);

            //            break;
            //        }
            //    case PageFittingType.ViewerHeight:
            //        {
            //            float firstPageHeight = referencePageSize.y;
            //            float viewportHeight = m_Internal.Viewport.rect.size.y;
            //            m_ZoomToGo = viewportHeight / firstPageHeight;

            //            break;
            //        }f
            //    case PageFittingType.WholePage:
            //        {
            //            float firstPageWidth = referencePageSize.x;
            //            float firstPageHeight = referencePageSize.y + 2.0f * m_VerticalMarginBetweenPages;
            //            float viewportWidth = m_Internal.Viewport.rect.size.x;
            //            float viewportHeight = m_Internal.Viewport.rect.size.y;

            //            m_ZoomToGo = Mathf.Min(viewportWidth / (firstPageWidth * 2), viewportHeight / firstPageHeight);

            //            break;
            //        }
            //    case PageFittingType.Zoom:
            //        {
            //            break;
            //        }
            //}
        }

        // 현재 로드된 문서 닫음
        public void CloseDocument()
        {
            if (m_IsLoaded)
            {
                CleanUp();
            }
        }

        // 현재 파일의 위치를 반환
        public string GetFileLocation()
        {
            switch (m_FileSource)
            {
                case FileSourceType.FilePath:
                    return m_FilePath;

                case FileSourceType.Resources:
                    string folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    return (folder + m_FileName).Replace("//", "/").Replace(@"\\", @"/").Replace(@"\", @"/");

                case FileSourceType.StreamingAssets:
                    folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    string location = ("/" + folder + m_FileName).Replace("//", "/")
                        .Replace(@"\\", @"/")
                        .Replace(@"\", @"/");
                    return Application.streamingAssetsPath + location;

                case FileSourceType.PersistentData:
                    folder = m_Folder + "/";
                    if (string.IsNullOrEmpty(m_Folder))
                        folder = "";

                    location = ("/" + folder + m_FileName).Replace("//", "/")
                        .Replace(@"\\", @"/")
                        .Replace(@"\", @"/");
                    return Application.persistentDataPath + location;

                case FileSourceType.Web:
                    return m_FileURL;

                default:
                    return "";
            }
        }

        // 다음 페이지로 이동
        public void GoToNextPage()
        {
            // 현재 문서가 없거나 로드되지 않은 경우 return;
            if (m_Document == null || !m_Document.IsValid)
                return;

            // 현재 가장 많이 보이는 페이지의 인덱스 가져옴.
            int mostVisiblePage = GetMostVisiblePageIndex();

            // 다음 페이지 존재하는 경우
            // 페이지 2개 씩 이동.
            if (mostVisiblePage + 2 < m_PageCount)
            {
                // 다음 페이지로 이동.
                GoToPage(mostVisiblePage + 2);
            }
            else
            {
                // 마지막 페이지 일 경우 맨 아래로 이동.
                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y);
            }
        }

        // 다음 검색 결과로 이동
        public void GoToNextSearchResult()
        {
            // 현재 문서가 없거나 로드되지 않은 경우 return;
            if (m_Document == null || !m_Document.IsValid)
                return;

            // 검색결과가 있고 현재 검색 결과 인덱스가 유효한 경우
            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                ++m_CurrentSearchResultIndex;
                ++m_CurrentSearchResultIndexWithinCurrentPage;

                int oldPageIndex = m_CurrentSearchResult.PageIndex;

                if (m_CurrentSearchResultIndexWithinCurrentPage >= m_SearchResults[m_CurrentSearchResult.PageIndex].Count)
                {
                    int nextPage = m_CurrentSearchResult.PageIndex + 1;
                    while (nextPage < m_PageCount - 1 && m_SearchResults[nextPage].Count == 0)
                    {
                        ++nextPage;
                    }
                    // 다음 페이지에 검색 결과가 있는 경우
                    if (nextPage <= m_PageCount - 1 && m_SearchResults[nextPage].Count > 0)
                    {
                        m_CurrentSearchResultIndexWithinCurrentPage = 0;

                        m_CurrentSearchResult = m_SearchResults[nextPage][0];

                        if (oldPageIndex != nextPage)
                        {
                            GoToPage(nextPage);
                        }

                    }
                    else
                    {
                        --m_CurrentSearchResultIndexWithinCurrentPage;
                        --m_CurrentSearchResultIndex;

                        if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                            GoToPage(m_CurrentSearchResult.PageIndex);
                    }
                }
                else
                {
                    m_CurrentSearchResult =
                        m_SearchResults[m_CurrentSearchResult.PageIndex][m_CurrentSearchResultIndexWithinCurrentPage];

                    if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                        GoToPage(m_CurrentSearchResult.PageIndex);
                }
            }
        }

        // 지정된 페이지로 이동
        public void GoToPage(int pageIndex)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            // pageIndex가 유효한 범위 내에 있는지 확인 후 보정
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            else if (pageIndex > m_PageCount - 1)
            {
                pageIndex = m_PageCount - 1;
            }

            m_Internal.PageInputField.text = (pageIndex + 1).ToString();
            m_Internal.PageContainer.anchoredPosition = new Vector2(m_Internal.PageContainer.anchoredPosition.x,
                m_PageOffsets[pageIndex] - m_PageSizes[pageIndex].y * 0.5f);
            m_Internal.PageContainer.anchoredPosition -= m_VerticalMarginBetweenPages * Vector2.up;

            // 페이지 수 레이블 업데이트
            SetPageCountLabel(pageIndex, m_PageCount);

            // 페이지 컨테이너의 위치가 유효한지 확인.
            EnsureValidPageContainerPosition();
        }

        // 이전 페이지로 이동
        public void GoToPreviousPage()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            // 현재 가장 많이 보이는 페이지 인덱스 가져옴.
            int mostVisiblePage = GetMostVisiblePageIndex();

            if (mostVisiblePage - 2 >= 0)
            {
                GoToPage(mostVisiblePage - 2);
            }
            else
            {
                m_Internal.PageContainer.anchoredPosition = Vector2.zero;
            }
        }

        // 이전 검색 결과로 이동.
        public void GoToPreviousSearchResult()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (m_SearchResults != null && m_SearchResults.Length > 0 && m_CurrentSearchResultIndex > 0)
            {
                --m_CurrentSearchResultIndex;
                --m_CurrentSearchResultIndexWithinCurrentPage;

                int oldPageIndex = m_CurrentSearchResult.PageIndex;

                if (m_CurrentSearchResultIndexWithinCurrentPage < 0)
                {
                    int prevPage = m_CurrentSearchResult.PageIndex - 1;
                    while (prevPage >= 0 && m_SearchResults[prevPage].Count == 0)
                    {
                        --prevPage;
                    }

                    if (prevPage >= 0 && m_SearchResults[prevPage].Count > 0)
                    {
                        m_CurrentSearchResultIndexWithinCurrentPage = m_SearchResults[prevPage].Count - 1;
                        m_CurrentSearchResult = m_SearchResults[prevPage][m_SearchResults[prevPage].Count - 1];

                        if (oldPageIndex != prevPage)
                        {
                            GoToPage(prevPage);
                        }
                    }
                    else
                    {
                        ++m_CurrentSearchResultIndexWithinCurrentPage;
                        ++m_CurrentSearchResultIndex;

                    }
                }
                else
                {
                    m_CurrentSearchResult =
                        m_SearchResults[m_CurrentSearchResult.PageIndex][m_CurrentSearchResultIndexWithinCurrentPage];

                    if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                        GoToPage(m_CurrentSearchResult.PageIndex);
                }
            }
        }

        // 다운로드 취소
        public void CancelDownload()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            StopCoroutine(DownloadFileFromWWW());

            m_Internal.DownloadDialog.gameObject.SetActive(false);

            m_DownloadCanceled = true;

            NotifyDownloadCancelled();
#endif
        }

        // 페이지 입력 필드의 편집이 끝났을 때 호출.
        public void OnPageEditEnd()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            if (string.IsNullOrEmpty(m_Internal.PageInputField.text))
                return;

            int pageIndex = int.Parse(m_Internal.PageInputField.text) - 1;

            GoToPage(pageIndex);
        }

        // 비밀번호 대화 상자에서 취소 버튼이 클릭되었을 때 호출
        public void OnPasswordDialogCancelButtonClicked()
        {
            m_InvalidPasswordMessageVisisble = false;
            m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
            m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;

            NotifyPasswordCancelled();

            CleanUp();
        }

        // 비밀번호 대화 상자에서 확인 버튼이 클릭되었을 때 호출
        public void OnPasswordDialogOkButtonClicked()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            m_Password = m_Internal.PasswordInputField.text;

            if (TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_Password))
            {
                m_Internal.PasswordDialog.gameObject.SetActive(false);

                m_InvalidPasswordMessageVisisble = false;
                m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
                m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;

                m_Internal.PasswordInputField.text = "";
            }
            else
            {
                m_InvalidPasswordMessageVisisble = true;
                m_Internal.InvalidPasswordImage.gameObject.SetActive(true);
                m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>().alpha = 1.0f;
                m_InvalidPasswordMessageDelay = m_InvalidPasswordMessageDelayBeforeFade;

                m_Internal.PasswordInputField.Select();
            }
#endif
        }

        // 현재 페이지 인덱스로 문서를 다시 로드
        public void ReloadDocument(int pageIndex = 0)
        {
            LoadDocument(pageIndex);
        }

        // 문서를 파일로 저장
        public bool SaveDocumentAsFile(string path)
        {
            if (m_Document == null || m_Document.DocumentBuffer == null)
            {
                Debug.LogError("Error while saving document: there is no document loaded.");
                return false;
            }

            if (!new Uri(path).IsWellFormedOriginalString())
            {
                Debug.LogError("Error while saving document: the path is not well formed => " + path);
                return false;
            }

            try
            {
                FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                stream.Write(m_Document.DocumentBuffer, 0, m_Document.DocumentBuffer.Length);
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception while saving document: " + ex);
            }

            return false;
        }

        // 검색 결과를 설정
        public void SetSearchResults(IList<PDFSearchResult>[] searchResults)
        {
            m_SearchResults = searchResults;

            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                m_CurrentSearchResultIndex = 0;
                m_CurrentSearchResultIndexWithinCurrentPage = 0;

                for (int i = 0; i < m_PageCount; ++i)
                {
                    if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                    {
                        m_CurrentSearchResult = m_SearchResults[i][0];
                        break;
                    }
                }
            }
            else
            {

                m_CurrentSearchResult = new PDFSearchResult(-1, 0, 0);
                m_CurrentSearchResultIndex = 0;
            }

            AdjustCurrentSearchResultDisplayed();

            m_UpdateChangeDelay = 0.25f;
        }

        // 현재 로드된 문서를 언로드
        public void UnloadDocument()
        {
            if (m_IsLoaded)
                CleanUp();
        }

        // 확대
        public void ZoomIn()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), true);
        }

        // 축소
        public void ZoomOut()
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), false);
        }

        // 현재 검색 결과 표시
        private void AdjustCurrentSearchResultDisplayed()
        {
            if (m_SearchResults != null && m_SearchResults.Length > 0)
            {
                if (!m_CurrentPageRange.ContainsPage(m_CurrentSearchResult.PageIndex))
                {
                    int minPage = m_CurrentPageRange.m_From;
                    int maxPage = m_CurrentPageRange.m_To;

                    bool minFound = false;
                    bool maxFound = false;

                    for (int i = minPage; i >= 0; --i)
                    {
                        if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                        {
                            minFound = true;
                            minPage = i;
                            break;
                        }
                    }

                    for (int i = maxPage; i < m_PageCount; ++i)
                    {
                        if (m_SearchResults[i] != null && m_SearchResults[i].Count > 0)
                        {
                            maxFound = true;
                            maxPage = i;
                            break;
                        }
                    }

                    int disMinPage = Math.Abs(m_CurrentPageRange.m_From - minPage);
                    int disMaxPage = Math.Abs(maxPage - m_CurrentPageRange.m_To);

                    int nearestPage = -1;

                    if (disMinPage <= disMaxPage)
                    {
                        if (minFound)
                        {
                            nearestPage = minPage;
                        }
                        else if (maxFound)
                        {
                            nearestPage = maxPage;
                        }
                    }
                    else
                    {
                        if (maxFound)
                        {
                            nearestPage = maxPage;
                        }
                        else if (minFound)
                        {
                            nearestPage = minPage;
                        }
                    }

                    int count = 0;

                    for (int i = 0; i < nearestPage; ++i)
                    {
                        count += m_SearchResults[i].Count;
                    }

                    if (minFound || maxFound)
                    {
                        if (m_CurrentPageRange.ContainsPage(nearestPage)
                            || nearestPage >= m_CurrentPageRange.m_To)
                        {
                            m_CurrentSearchResult = m_SearchResults[nearestPage][0];
                            m_CurrentSearchResultIndex = count;
                            m_CurrentSearchResultIndexWithinCurrentPage = 0;
                        }
                        else
                        {
                            m_CurrentSearchResult = m_SearchResults[nearestPage][m_SearchResults[nearestPage].Count - 1];
                            m_CurrentSearchResultIndex = count + m_SearchResults[nearestPage].Count - 1;
                            m_CurrentSearchResultIndexWithinCurrentPage = m_SearchResults[nearestPage].Count - 1;
                        }
                    }
                }
            }
        }

        // 현재 로드된 문서 정리 및 청소
        private void CleanUp()
        {
            if (m_Document != null)
                NotifyDocumentUnloaded(m_Document);

            m_Document = null;

            if (m_PageTextureHolders != null)
            {
                foreach (PDFPageTextureHolder holder in m_PageTextureHolders)
                {
                    if (holder.Texture != null)
                    {
                        Texture2D tex = holder.Texture;
                        holder.Texture = null;

                        Destroy(tex);
                    }

                    if (holder.Page.name != "Page")
                    {
                        Destroy(holder.Page);
                    }
                    else
                    {
                        holder.Page.GetComponent<PDFViewerPage>().ClearCache();
                    }
                }
            }

#if !UNITY_WEBGL
            m_Internal.SearchPanel.GetComponent<PDFSearchPanel>().Close();
#endif

            m_IsLoaded = false;

            m_Internal.PageContainer.anchoredPosition = Vector2.zero;
            m_Internal.PageContainer.sizeDelta = Vector2.zero;
            UpdateScrollBarVisibility();
            EnsureValidPageContainerPosition();

            m_ZoomToGo = m_StartZoom;
            m_PageSizes = null;
            m_NormalPageSizes = null;
            m_PageOffsets = null;
            m_PageCount = 0;
            m_PreviousZoom = 0.0f;
            m_PreviousZoomToGo = 0.0f;
            m_PageTextureHolders = null;
            m_CurrentPageRange = null;
            m_PreviousMostVisiblePage = -1;

            OverlayVisible = false;
            m_InvalidPasswordMessageVisisble = false;
            m_Internal.Overlay.gameObject.SetActive(false);
            m_Internal.PasswordDialog.gameObject.SetActive(false);
            m_Internal.DownloadDialog.gameObject.SetActive(false);

            m_LastSetLabelPageIndex = null;
            m_LastSetLabelPageCount = null;
            m_Internal.PageCountLabel.text = "";
            m_Internal.PageZoomLabel.text = "";
            m_Internal.PageInputField.text = "";
        }

        // 문서를 로드
        private void CommonLoad(byte[] specifiedBuffer = null)
        {
            UpdateScrollBarVisibility();

            m_IsLoaded = false;

            if (m_FileSource == FileSourceType.None)
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.alpha = OverlayAlpha;
                return;
            }

            if (m_FileSource != FileSourceType.DocumentObject)
                m_SuppliedDocument = null;

#if UNITY_WEBGL && !UNITY_EDITOR
			StartCoroutine(LoadDocument_WebGL(specifiedBuffer));

			return;
#else
            byte[] buffer = specifiedBuffer;

            if (m_FileSource == FileSourceType.DocumentObject)
            {
                TryLoadWithSpecifiedDocument(m_SuppliedDocument);
            }
            else if (m_FileSource == FileSourceType.FilePath)
            {
                buffer = File.ReadAllBytes(GetFileLocation());
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.Resources)
            {
                buffer = LoadAssetBytesFromResources(GetFileLocation());
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.StreamingAssets)
            {

#if UNITY_ANDROID && !UNITY_EDITOR
                StartCoroutine(DownloadFileFromWWW());
#else
                string location = GetFileLocation();
                if (File.Exists(location))
                    buffer = File.ReadAllBytes(location);
                OnLoadingBufferFinished(buffer);
#endif
            }
            else if (m_FileSource == FileSourceType.PersistentData)
            {
                string location = GetFileLocation();
                if (File.Exists(location))
                    buffer = File.ReadAllBytes(location);
                OnLoadingBufferFinished(buffer);
            }
            else if (m_FileSource == FileSourceType.Web)
            {
                StartCoroutine(DownloadFileFromWWW());
            }
            else if (m_FileSource == FileSourceType.Bytes)
            {
                if (buffer != null)
                {
                    OnLoadingBufferFinished(buffer);
                }
                else if (BytesSupplierComponent != null)
                {
                    MethodInfo methodInfo = BytesSupplierComponent.GetType().GetMethod(BytesSupplierFunctionName);

                    if (methodInfo != null)
                    {
                        buffer = (byte[])methodInfo.Invoke(BytesSupplierComponent, null);
                    }

                    if (buffer != null)
                    {
                        OnLoadingBufferFinished(buffer);
                    }
                }

                if (buffer == null)
                {
                    NotifyDocumentLoadFailed();
                }
            }
            else if (m_FileSource == FileSourceType.Asset)
            {
                if (m_PDFAsset != null && m_PDFAsset.m_FileContent != null && m_PDFAsset.m_FileContent.Length > 0)
                {
                    OnLoadingBufferFinished(m_PDFAsset.m_FileContent);
                }
                else
                {
                    NotifyDocumentLoadFailed();
                }
            }
#endif
        }

        // 페이지 간의 오프셋 계산.
        private void ComputePageOffsets()
        {
            //// 원래 코드
            //float totalOffset = m_VerticalMarginBetweenPages;

            //m_PageOffsets = new float[m_PageCount];

            //for (int i = 0; i < m_PageCount; ++i)
            //{
            //    m_PageOffsets[i] = totalOffset + m_PageSizes[i].y * 0.5f;

            //    totalOffset += m_VerticalMarginBetweenPages + m_PageSizes[i].y;
            //}
            Debug.Log("ComputePageOffsets() 함수 실행");
            // 수정 코드
            float totalOffset = m_VerticalMarginBetweenPages;

            m_PageOffsets = new float[m_PageCount];

            for (int i = 0; i < m_PageCount; ++i)
            {
                if (i % 2 == 0) // 짝수 페이지
                {
                    m_PageOffsets[i] = totalOffset + m_PageSizes[i].y * 0.5f;
                }
                else // 홀수 페이지
                {
                    m_PageOffsets[i] = totalOffset + m_PageSizes[i - 1].y * 0.5f;
                    totalOffset += m_VerticalMarginBetweenPages + m_PageSizes[i].y;
                }

                //totalOffset += m_VerticalMarginBetweenPages + m_PageSizes[i].y;
            }
        }

        // 각 페이지의 크기 계산.
        private void ComputePageSizes()
        {
            // 원래 코드
            m_PageCount = m_Document.GetPageCount();

            m_PageSizes = new Vector2[m_PageCount];

            for (int i = 0; i < m_PageCount; ++i)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                            m_PageSizes[i] = m_NormalPageSizes[i] * m_ZoomFactor;
#else
                float w = m_Document.GetPageWidth(i) * m_ZoomFactor;
                float h = m_Document.GetPageHeight(i) * m_ZoomFactor;

                m_PageSizes[i] = new Vector2(w, h);
#endif
            }


            Debug.Log("ComputePageSizes() 실행");

            // 수정 코드
            //m_PageCount = m_Document.GetPageCount();

            //m_PageSizes = new Vector2[m_PageCount];

            //for (int i = 0; i < m_PageCount; ++i)
            //{
            //    // 페이지 크기를 가져와서 너비를 두 배로 늘림
            //    float w = m_Document.GetPageWidth(i) * m_ZoomFactor * 2;
            //    float h = m_Document.GetPageHeight(i) * m_ZoomFactor;

            //    m_PageSizes[i] = new Vector2(w, h);
            //}
        }

        // WebGL 환경에서 문서 로드
#if UNITY_WEBGL && !UNITY_EDITOR
        IEnumerator LoadDocument_WebGL(byte[] specifiedBuffer = null)
        {
            PDFJS_Promise<PDFDocument> documentPromise = null;

            byte[] buffer = specifiedBuffer;

            bool fromUrl = false;
            
            switch (m_FileSource)
            {
                case FileSourceType.DocumentObject:
	                StartCoroutine(LoadWithDocument(m_SuppliedDocument));
                    yield break;
                case FileSourceType.Asset:
                    if (m_PDFAsset.m_FileContent == null || m_PDFAsset.m_FileContent.Length == 0)
                        yield break;

                    documentPromise = PDFDocument.LoadDocumentFromBytesAsync(m_PDFAsset.m_FileContent);
                    break;
                case FileSourceType.Bytes:
                    if (buffer != null)
                    {
                        documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    }
                    else if (BytesSupplierComponent != null)
                    {
                        MethodInfo methodInfo = BytesSupplierComponent.GetType().GetMethod(BytesSupplierFunctionName);
                        
                        if (methodInfo != null)
                            buffer = (byte[])methodInfo.Invoke(BytesSupplierComponent, null);

                        if (buffer != null)
                            documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    }

                    if (buffer == null)
                        yield break;
                    break;
                case FileSourceType.Resources:
                    buffer = LoadAssetBytesFromResources(GetFileLocation());

                    if (buffer != null)
                        documentPromise = PDFDocument.LoadDocumentFromBytesAsync(buffer);
                    else
                        yield break;
                    break;
                case FileSourceType.Web:
                case FileSourceType.FilePath:
                case FileSourceType.StreamingAssets:
                case FileSourceType.PersistentData:
                    documentPromise = PDFDocument.LoadDocumentFromUrlAsync(GetFileLocation());
                    fromUrl = true;
                    
                    break;
            }
             
            if (!fromUrl)
            {
                while (!documentPromise.HasFinished)
                    yield return null;
            }
            else
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.GetComponent<CanvasGroup>().alpha = 0.0f;

                m_Internal.DownloadDialog.gameObject.SetActive(true);

                m_Internal.DownloadSourceLabel.text = GetFileLocation();

                m_Internal.ProgressRect.sizeDelta = new Vector2(0.0f, m_Internal.ProgressRect.sizeDelta.y);
                m_Internal.ProgressLabel.text = "0%";

                while (!documentPromise.HasFinished)
                {
                    SetProgress(documentPromise.Progress);

                    yield return null;
                }

                m_Internal.DownloadDialog.gameObject.SetActive(false);
            }

            if (documentPromise.HasSucceeded)
            {
	            StartCoroutine(LoadWithDocument(documentPromise.Result));
            }
            else
            {
                NotifyDocumentLoadFailed();
            }
        }

        private IEnumerator LoadWithDocument(PDFDocument document)
        {
	        m_Document = document;

	        m_NormalPageSizes = new Vector2[m_Document.GetPageCount()];

	        for (int i = 0; i < m_NormalPageSizes.Length; ++i)
	        {
		        PDFJS_Promise<PDFPage> pagePromise = m_Document.GetPageAsync(i);

		        while (!pagePromise.HasFinished)
			        yield return null;

		        if (pagePromise.HasSucceeded)
		        {
			        PDFPage page = pagePromise.Result;

			        m_NormalPageSizes[i] = page.GetPageSize(1.0f);
		        }
		        else
		        {
			        NotifyDocumentLoadFailed();
			        yield break;
		        }
	        }

	        TryLoadWithSpecifiedDocument(m_Document);
        }
#endif

        // 일반 환경에서 문서 로드
#if !UNITY_WEBGL || UNITY_EDITOR
        private IEnumerator DownloadFileFromWWW()
        {
            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = 0.0f;

            m_Internal.DownloadDialog.gameObject.SetActive(true);

            if (m_FileSource == FileSourceType.Web)
            {
                m_Internal.DownloadSourceLabel.text = GetFileLocation();
            }
            else
            {
                m_Internal.DownloadSourceLabel.text = "";
            }

            m_Internal.ProgressRect.sizeDelta = new Vector2(0.0f, m_Internal.ProgressRect.sizeDelta.y);
            m_Internal.ProgressLabel.text = "0%";

            PDFWebRequest www = new PDFWebRequest(GetFileLocation());
            www.SendWebRequest();

            m_DownloadCanceled = false;

            while (!www.isDone && !m_DownloadCanceled)
            {
                SetProgress(www.progress);
                yield return null;
            }

            if (!m_DownloadCanceled && string.IsNullOrEmpty(www.error) && www.isDone)
            {
                SetProgress(1.0f);

                OnLoadingBufferFinished(www.bytes);
            }
            else if (m_DownloadCanceled || !string.IsNullOrEmpty(www.error))
            {
                NotifyDocumentLoadFailed();
            }

            www.Dispose();
            www = null;

            m_Internal.DownloadDialog.gameObject.SetActive(false);
        }
#endif

        // 페이지 컨테이너의 위치를 조정
        private void EnsureValidPageContainerPosition()
        {
            //// 만약 페이지 크기(m_PageSizes)가 아직 초기화되지 않았거나,
            //// 문서의 전체 너비가 화면 뷰포트의 너비보다 작거나 같다면:
            if (m_PageSizes == null || GetDocumentSize().x <= m_Internal.Viewport.rect.size.x)
            {
                // 페이지 컨테이너의 가로 위치를 0으로 설정합니다.
                // 이렇게 함으로써 페이지 컨테이너는 뷰포트의 왼쪽 끝에 정렬됩니다.
                m_Internal.PageContainer.anchoredPosition = new Vector2(0.0f,
                    m_Internal.PageContainer.anchoredPosition.y);
            }

            // 만약 페이지 컨테이너의 상단 부분이 뷰포트의 하단을 넘어가면
            if (m_Internal.PageContainer.anchoredPosition.y >
                m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y)
            {
                // 페이지 컨테이너의 상단 위치를,
                // 페이지 컨테이너의 전체 높이에서 뷰포트의 높이를 뺀 값으로 설정합니다.
                // 이렇게 함으로써 페이지 컨테이너는 뷰포트의 하단 끝에 정렬됩니다.

                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_Internal.PageContainer.sizeDelta.y - m_Internal.Viewport.rect.size.y);

            }
            // 만약 페이지 컨테이너의 하단 부분이 화면 위로 넘어가면:
            if (m_Internal.PageContainer.anchoredPosition.y < 0.0f)
            {
                // 페이지 컨테이너의 하단 위치를 0으로 설정합니다.
                // 이렇게 함으로써 페이지 컨테이너는 화면 상단에 정렬됩니다.
                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x, 0.0f);
            }
        }

        // 문서 전체 크기를 반환
        private Vector2 GetDocumentSize()
        {
            Vector2 size = new Vector2(0.0f, 0.0f);

            foreach (Vector2 s in m_PageSizes)
            {
                if (s.x > size.x)
                {
                    size.x = s.x;
                }
            }

            size.y = m_PageOffsets[m_PageCount - 1] + m_PageSizes[m_PageCount - 1].y * 0.5f;

            size.x += 0.0f * m_VerticalMarginBetweenPages;
            size.y += 1.0f * m_VerticalMarginBetweenPages;

            return size;
        }

        // 가장 많이 보이는 페이지의 인덱스 가져옴.
        private int GetMostVisiblePageIndex()
        {

            // 원래 코드
            int mostVisibleIndex = -1;
            float mostVisibleArea = 0.0f;

            for (int i = m_CurrentPageRange.m_From; i < m_CurrentPageRange.m_To; ++i)
            {
                RectTransform page = (RectTransform)m_Internal.PageContainer.GetChild(i);
                float area = PDFInternalUtils.CalculateRectTransformIntersectArea(page, m_Internal.Viewport);

                if (area > page.sizeDelta.x * page.sizeDelta.y * 0.4f)
                    return i;

                if (area > mostVisibleArea)
                {
                    mostVisibleIndex = i;
                    mostVisibleArea = area;
                }
            }

            // 수정 코드
            //int mostVisibleIndex = -1;
            //float mostVisibleArea = 0.0f;

            //for (int i = (m_CurrentPageRange.m_From % 2 == 0 ? m_CurrentPageRange.m_From : m_CurrentPageRange.m_From + 1); i < m_CurrentPageRange.m_To; i += 2)
            //{
            //    RectTransform page = (RectTransform)m_Internal.PageContainer.GetChild(i);
            //    float area = PDFInternalUtils.CalculateRectTransformIntersectArea(page, m_Internal.Viewport);

            //    if (area > page.sizeDelta.x * page.sizeDelta.y * 0.4f)
            //        return i;

            //    if (area > mostVisibleArea)
            //    {
            //        mostVisibleIndex = i;
            //        mostVisibleArea = area;
            //    }
            //}

            return mostVisibleIndex;
        }

        // 두 개의 사각형이 교차하는지 확인
        private static bool Intersect(Rect box0, Rect box1)
        {
            if (box0.xMax < box1.xMin || box0.xMin > box1.xMax)
                return false;
            if (box0.yMax < box1.yMin || box0.yMin > box1.yMax)
                return false;

            return true;
        }

        // 현재 화면에서 보이는 페이지 범위를 가져옴.
        private PDFPageRange GetVisiblePageRange()
        {
            if (m_PageCount == 0)
                throw new Exception("There is no document loaded.");

            PDFPageRange pageRange = new PDFPageRange();

            for (int i = 0; i < m_PageCount; ++i)
            {
                RectTransform rt = (RectTransform)m_Internal.PageContainer.GetChild(i);

                Rect pageRect = new Rect(-m_Internal.PageContainer.anchoredPosition - rt.anchoredPosition, rt.rect.size);
                Rect viewportRect = new Rect(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), m_Internal.Viewport.rect.size);

                pageRect.position = Vector2.zero;
                viewportRect.position = Vector2.zero;

                pageRect.center = -m_Internal.PageContainer.anchoredPosition - rt.anchoredPosition;
                viewportRect.center = new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f);

                if (Intersect(pageRect, viewportRect))

                {
                    if (pageRange.m_From == -1)
                    {
                        pageRange.m_From = i;
                    }
                    else
                    {
                        pageRange.m_To = i + 1;
                    }
                }
                else if (pageRange.m_From != -1)
                {
                    break;
                }
            }

            if (pageRange.m_From != -1 && pageRange.m_To == -1)
            {
                pageRange.m_To = pageRange.m_From + 1;
            }
            return pageRange;
        }

        // 페이지의 텍스처 홀더를 생성
        private void InstantiatePageTextureHolders()
        {
            // 원래 코드
            if (m_PageTextureHolders == null)
            {
                m_PageTextureHolders = new PDFPageTextureHolder[m_PageCount];

                for (int i = 0; i < m_PageCount; ++i)
                {
                    GameObject page = i == 0
                        ? m_Internal.PageSample.gameObject
                        : Instantiate(m_Internal.PageSample.gameObject);

                    page.transform.SetParent(m_Internal.PageSample.transform.parent, false);
                    page.transform.localScale = Vector3.one;
                    page.transform.localRotation = Quaternion.identity;

                    PDFPageTextureHolder textureHolder = new PDFPageTextureHolder
                    {
                        PageIndex = i,
                        Page = page,
                        Viewer = this
                    };

                    m_PageTextureHolders[i] = textureHolder;
                }
            }
            // 수정 코드           

            //if (m_PageTextureHolders == null)
            //{
            //    // 두 배로 늘려 초기화
            //    m_PageTextureHolders = new PDFPageTextureHolder[m_PageCount * 2];

            //    // 각 페이지에 대해 두 개의 페이지 홀더 생성
            //    for (int i = 0; i < m_PageCount * 2; ++i)
            //    {
            //        // 첫 번째 페이지인 경우, 샘플 페이지 사용
            //        // 그렇지 않다면, 샘플 페이지 복제하여 새로운 페이지 생성

            //        GameObject page = i == 0
            //            ? m_Internal.PageSample.gameObject
            //            : Instantiate(m_Internal.PageSample.gameObject);

            //        // 페이지의 부모를 설정하고 크기 및 회전을 초기화한다.
            //        page.transform.SetParent(m_Internal.PageSample.transform.parent, false);
            //        page.transform.localScale = Vector3.one;
            //        page.transform.localRotation = Quaternion.identity;

            //        // 페이지 텍스처 홀더를 초기화
            //        PDFPageTextureHolder textureHolder = new PDFPageTextureHolder
            //        {
            //            PageIndex = i,
            //            Page = page,
            //            Viewer = this
            //        };

            //        // 페이지 텍스처 홀더를 배열에 저장.
            //        m_PageTextureHolders[i] = textureHolder;
            //    }
            //}
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                PDFPageTextureHolder holder_ = m_PageTextureHolders[0];

                Color customColor = new Color(0.71f, 0.71f, 0.71f, 1.0f); // 0.71은 B7B7B7을 0에서 1 사이의 값으로 변환한 것입니다.

                RawImage rawImage = holder_.Page.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    rawImage.color = customColor; // B7B7B7 색상을 적용합니다.
                }
            }

            if (m_DelayedOnEnable)
            {
                m_DelayedOnEnable = false;

                if (m_LoadOnEnable && !m_IsLoaded)
                {
                    LoadDocument();
                }
                else
                {
                    // 오버레이 
                    OverlayVisible = true;
                    m_Internal.Overlay.gameObject.SetActive(true);
                    m_Internal.Overlay.alpha = OverlayAlpha;
                }
            }

            // 줌 동작처리.
            ProcessPinchZoom();
        }

        // 줌 동작 처리.
        private void ProcessPinchZoom()
        {
            if (m_GraphicRaycaster == null)
            {
                if (m_Canvas == null)
                    CacheCanvas();

                if (m_GraphicRaycaster == null)
                    return;
            }

            int validTouchCount = 0;

            // 터치 입력이 발생하면 처리합니다.
            if (Input.touchCount >= 1)
            {
                // 모든 터치 이벤트에 대해 반복합니다.
                foreach (Touch touch in Input.touches)
                {
                    // 현재 터치가 Unity UI 요소 위에 있는지 확인합니다.
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        // UI 이벤트 데이터를 생성하고 터치 위치를 설정합니다.
                        PointerEventData ped = new PointerEventData(null);
                        ped.position = touch.position;

                        // UI 요소와의 교차 여부를 검사하고 결과를 리스트에 저장합니다.
                        List<RaycastResult> results = new List<RaycastResult>();
                        m_GraphicRaycaster.Raycast(ped, results);

                        // 검사 결과를 반복하여 처리합니다.
                        foreach (RaycastResult result in results)
                        {
                            // 현재 PDFViewer 스크립트가 속한 객체인지 확인합니다.
                            if (result.gameObject.GetComponentInParent<PDFViewer>() == this)
                            {
                                // 유효한 터치의 개수를 증가시킵니다.
                                ++validTouchCount;

                                // 현재 PDFViewer 스크립트가 속한 객체임이 확인되면 더 이상 검사하지 않습니다.
                                break;
                            }
                        }
                    }
                }
            }

            if (validTouchCount >= 2)
            {
                // 이전 터치의 개수가 2개 미만이었다면 실행됩니다.
                if (m_PreviousTouchCount < 2)
                {
                    // 핀치 줌 시작 시 현재 줌 팩터를 저장합니다.
                    m_PinchZoomStartZoomFactor = ZoomFactor;

                    // 스크롤 기능을 잠금 상태로 설정하고, 관성 스크롤을 비활성화합니다.
                    ScrollRect scrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();
                    scrollRect.inertia = false;
                    scrollRect.horizontal = false;
                    scrollRect.vertical = false;

                    // 스크롤 기능 잠금을 해제하는 코루틴을 중지합니다.
                    StopCoroutine(DelayedUnlockScrollRect());
                }

                // 첫 번째와 두 번째 터치의 위치 차이를 계산합니다.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // 이전 터치의 개수가 2개 미만이었다면 실행됩니다.
                if (m_PreviousTouchCount < 2)
                    m_PinchZoomStartDeltaMag = touchDeltaMag;
                else
                    // 핀치 줌 중에 현재 줌 팩터를 업데이트합니다.
                    ZoomFactor = m_PinchZoomStartZoomFactor / (m_PinchZoomStartDeltaMag / touchDeltaMag);
            }
            else if (m_PreviousTouchCount >= 2)
            {
                // 터치의 개수가 2개 이상에서 2개 미만으로 감소한 경우 실행됩니다.
                StartCoroutine(DelayedUnlockScrollRect());
            }

            // 현재 터치의 개수를 저장합니다.
            m_PreviousTouchCount = validTouchCount;
        }

        // 핀치 줌 동착 후 ScrollRect를 잠금 해제하는 함수.
        private IEnumerator DelayedUnlockScrollRect()
        {
            while (Input.touchCount != 0)
                yield return null;

            ScrollRect scrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();

            scrollRect.inertia = true;
            scrollRect.horizontal = true;
            scrollRect.vertical = true;
        }

        // 리소스에서 에셋 바이트 로드
        private byte[] LoadAssetBytesFromResources(string path)
        {
            string fixedPath = path.Replace(".bytes", "");
            if (fixedPath.StartsWith("./"))
                fixedPath = fixedPath.Substring(2);

            TextAsset pdfAsset = Resources.Load(fixedPath, typeof(TextAsset)) as TextAsset;

            if (pdfAsset != null && pdfAsset.bytes != null && pdfAsset.bytes.Length > 0)
                return pdfAsset.bytes;

            return null;
        }

        // 현재 페이지 변경을 알림.
        private void NotifyCurrentPageChanged(int oldPageIndex, int newPageIndex)
        {
            if (OnCurrentPageChanged != null)
                OnCurrentPageChanged(this, oldPageIndex, newPageIndex);

            m_ThumbnailsViewer.OnCurrentPageChanged(newPageIndex);
        }

        // 컴포넌트가 비활성화될 때 호출.
        private void NotifyDisabled()
        {
            if (OnDisabled != null)
                OnDisabled(this);
        }

        // 문서가 성공적으로 로드되었음을 알림.
        private void NotifyDocumentLoaded(PDFDocument document)
        {
            EnsureValidPageContainerPosition();

            if (OnDocumentLoaded != null)
                OnDocumentLoaded(this, document);

            m_ThumbnailsViewer.OnDocumentLoaded(document);
            m_BookmarksViewer.OnDocumentLoaded(document);

            // 임시 테스트용.
            m_Internal.PageContainer.sizeDelta = new Vector2(m_Internal.PageContainer.sizeDelta.x * 2, m_Internal.PageContainer.sizeDelta.y);

            StartCoroutine(FirstPageColor());
        }

        // 임시 테스트용 첫페이지 색깔 바꾸기
        IEnumerator FirstPageColor()
        {
            yield return new WaitForSeconds(0.3f);

            PDFPageTextureHolder holder_ = m_PageTextureHolders[0];

            Color customColor = new Color(111f / 255f, 198f / 255f, 243f / 255f, 1.0f); // 0.71은 B7B7B7을 0에서 1 사이의 값으로 변환한 것입니다.

            RawImage rawImage = holder_.Page.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.color = customColor; // B7B7B7 색상을 적용합니다.
            }
        }


        // 문서 로드가 실패했음을 알림.
        private void NotifyDocumentLoadFailed()
        {
            if (OnDocumentLoadFailed != null)
                OnDocumentLoadFailed(this);
        }

        // 문서가 언로드되었음을 알림.
        private void NotifyDocumentUnloaded(PDFDocument document)
        {
            if (OnDocumentUnloaded != null)
                OnDocumentUnloaded(this, document);

            m_ThumbnailsViewer.OnDocumentUnloaded();
            m_BookmarksViewer.OnDocumentUnloaded();
        }

        // 다운로드가 취소되었음을 알림.
        private void NotifyDownloadCancelled()
        {
            if (OnDownloadCancelled != null)
                OnDownloadCancelled(this);
        }

        // 암호 입력이 취소되었음 알림.
        private void NotifyPasswordCancelled()
        {
            if (OnPasswordCancelled != null)
                OnPasswordCancelled(this);
        }

        // 줌 레벨이 변경되었음을 알림.
        private void NotifyZoomChanged(float oldZoom, float newZoom)
        {
            if (OnZoomChanged != null)
                OnZoomChanged(this, oldZoom, newZoom);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_UnloadOnDisable && m_IsLoaded)
            {
                if (m_Renderer != null)
                    m_Renderer.Dispose();
                m_Renderer = null;

                CleanUp();
            }

            NotifyDisabled();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_DelayedOnEnable = true;

            // 썸네일 뷰어와 북마크 뷰어 설정.
            if (m_ThumbnailsViewer == null)
                m_ThumbnailsViewer = m_Internal.LeftPanel.ThumbnailsViewer;
            if (m_BookmarksViewer == null)
                m_BookmarksViewer = m_Internal.LeftPanel.Bookmarks.GetComponent<PDFBookmarksViewer>();

            if (!m_ShowBookmarksViewer && m_ShowThumbnailsViewer)
                m_Internal.LeftPanel.OnThumbnailsTabClicked();
            else if (m_ShowBookmarksViewer && !m_ShowThumbnailsViewer)
                m_Internal.LeftPanel.OnBookmarksTabClicked();
            else
                m_Internal.LeftPanel.OnBookmarksTabClicked();


            m_ThumbnailsViewer.DoOnEnable();
            m_BookmarksViewer.DoOnEnable();

#if UNITY_WEBGL
            m_Internal.SearchPanel.gameObject.SetActive(false);

            int c = m_Internal.TopPanel.childCount;
            for (int i = 0; i < c; ++i)
            {
                Transform t = m_Internal.TopPanel.GetChild(i);
                if (t.name == "SearchButton")
                    t.gameObject.SetActive(false);
            }
#endif
        }

        //일반환경에서 버퍼 로딩 완료
#if !UNITY_WEBGL || UNITY_EDITOR

        private void OnLoadingBufferFinished(byte[] buffer)
        {
            m_PendingDocumentBuffer = buffer;

            if (m_PendingDocumentBuffer != null && m_PendingDocumentBuffer.Length > 0)
            {
                if (!TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_Password))
                {
                    if (m_FileSource == FileSourceType.Asset)
                    {
                        if (!TryLoadDocumentWithBuffer(m_PendingDocumentBuffer, m_PDFAsset.m_Password))
                        {
                            ShowPasswordDialog();
                        }
                    }
                    else
                        ShowPasswordDialog();
                }
            }
            else
            {
                OverlayVisible = true;
                m_Internal.Overlay.gameObject.SetActive(true);
                m_Internal.Overlay.alpha = OverlayAlpha;
            }
        }
#endif

        // 페이지 카운트 레이블 설정
        private void SetPageCountLabel(int pageIndex, int pageCount)
        {
            if (m_LastSetLabelPageIndex == null || m_LastSetLabelPageIndex.Value != pageIndex
                || m_LastSetLabelPageCount == null || m_LastSetLabelPageCount.Value != pageCount)
            {
                m_Internal.PageCountLabel.text = "(" + pageIndex + " of " + pageCount + ")";

                m_LastSetLabelPageIndex = pageIndex;
                m_LastSetLabelPageCount = pageCount;
            }
        }

        // Progres bar 업데이트 함수
        private void SetProgress(float progress)
        {
            RectTransform rectTransform = (RectTransform)m_Internal.ProgressRect.parent.transform;
            if (rectTransform != null)
            {
                m_Internal.ProgressRect.sizeDelta = new Vector2(Mathf.Clamp01(progress) * rectTransform.sizeDelta.x,
                    m_Internal.ProgressRect.sizeDelta.y);
            }
            m_Internal.ProgressLabel.text = (int)(Mathf.Clamp01(progress) * 100) + "%";
        }

        // 줌 레이블 설정.
        private void SetZoomLabel()
        {
            m_Internal.PageZoomLabel.text = "(" + (int)Mathf.Round(m_ZoomFactor * 100.0f) + "%)";
        }

        // 패스워드 다이얼로그 표시
        private void ShowPasswordDialog()
        {
            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = 0.0f;

            m_Internal.PasswordDialog.gameObject.SetActive(true);
        }


        protected override void Start()
        {
            m_StartZoom = m_ZoomToGo;

            // 만약 m_LinksActionHandler가 null이라면 새로운 PDFViewerDefaultActionHandler를 할당하여 초기화합니다.
            m_LinksActionHandler = m_LinksActionHandler ?? new PDFViewerDefaultActionHandler();

            // 만약 m_BookmarksActionHandler가 null이라면 새로운 PDFViewerDefaultActionHandler를 할당하여 초기화합니다
            m_BookmarksActionHandler = m_BookmarksActionHandler ?? new PDFViewerDefaultActionHandler();
        }

        // 버퍼로 문서 로드 시도 함수.
#if !UNITY_WEBGL || UNITY_EDITOR
        private bool TryLoadDocumentWithBuffer(byte[] buffer, string password)
        {
            m_Document = new PDFDocument(buffer, password);

            return TryLoadWithSpecifiedDocument(m_Document);
        }
#endif
        // 지정된 문서로 로드 시도
        private bool TryLoadWithSpecifiedDocument(PDFDocument document)
        {
            m_Document = document;

            if (m_Document != null && m_Document.IsValid)
            {
                m_CurrentPageRange = new PDFPageRange();

                m_PageCount = m_Document.GetPageCount();

#if !UNITY_WEBGL || UNITY_EDITOR
                m_NormalPageSizes = new Vector2[m_PageCount];

                for (int i = 0; i < m_NormalPageSizes.Length; ++i)
                    m_NormalPageSizes[i] = m_Document.GetPageSize(i);
#endif

                m_Internal.ScrollRect.scrollSensitivity = m_ScrollSensitivity;

                m_PreviousPageFitting = m_PageFitting;
                AdjustZoomToPageFitting(m_PageFitting, m_NormalPageSizes[0]);
                m_ZoomFactor = m_ZoomToGo;

                ComputePageSizes();
                ComputePageOffsets();

                InstantiatePageTextureHolders();

                m_Internal.PageContainer.sizeDelta = GetDocumentSize();

                SetPageCountLabel(1, m_PageCount);
                SetZoomLabel();

                m_Internal.PageContainer.anchoredPosition = Vector2.zero;

                m_IsLoaded = true;

                UpdatePagesPlacement();

                UpdateScrollBarVisibility();
                EnsureValidPageContainerPosition();

                GoToPage(m_LoadAtPageIndex);

                m_LoadAtPageIndex = 0;

                NotifyDocumentLoaded(m_Document);

                UpdateBookmarksViewerVisibility(m_ShowBookmarksViewer);

                return true;
            }

            OverlayVisible = true;
            m_Internal.Overlay.gameObject.SetActive(true);
            m_Internal.Overlay.alpha = OverlayAlpha;

            m_IsLoaded = false;

            NotifyDocumentLoadFailed();

            return false;
        }

        private void LateUpdate()
        {

            // 비밀번호 캔버스
            if (m_InvalidPasswordMessageVisisble && m_InvalidPasswordMessageDelay >= 0.0f)
            {
                // 유효한 암호화된 메시지가 표시되고, 딜레이가 0 이상인 경우에 실행됩니다.
                m_InvalidPasswordMessageDelay = m_InvalidPasswordMessageDelay - Time.deltaTime;

                if (m_InvalidPasswordMessageDelay < 0.0f)
                    m_InvalidPasswordMessageDelay = 0.0f;

                // 암호화된 메시지의 캔버스 그룹을 가져옵니다.
                CanvasGroup messageCanvas = m_Internal.InvalidPasswordImage.GetComponent<CanvasGroup>();

                if (m_InvalidPasswordMessageDelay <= 0.0f)
                    // 메시지 캔버스의 알파 값을 조절하여 페이드 아웃 효과를 줍니다.
                    messageCanvas.alpha = Mathf.Clamp01(messageCanvas.alpha - Time.deltaTime);

                if (messageCanvas.alpha <= 0.0f)
                {
                    // 알파 값이 0 이하이면 실행되며, 메시지를 숨기고 초기화합니다.
                    m_InvalidPasswordMessageVisisble = false;
                    messageCanvas.alpha = 1.0f;
                    m_Internal.InvalidPasswordImage.gameObject.SetActive(false);
                }
            }

            // 오버레이
            if (OverlayVisible && !m_IsLoaded)
            {
                // 오버레이가 표시되고 문서가 로드되지 않은 경우 실행됩니다.
                CanvasGroup overlayCanvas = m_Internal.Overlay;

                // 오버레이의 알파 값을 조절하여 페이드 인 효과를 줍니다.
                overlayCanvas.alpha = Mathf.Clamp01(overlayCanvas.alpha + Time.deltaTime * 2.0f);

                if (overlayCanvas.alpha > OverlayAlpha)
                    overlayCanvas.alpha = OverlayAlpha;
            }
            else if (OverlayVisible && m_IsLoaded)
            {
                // 오버레이가 표시되고 문서가 로드된 경우 실행됩니다.
                CanvasGroup overlayCanvas = m_Internal.Overlay;

                // 오버레이의 알파 값을 조절하여 페이드 아웃 효과를 줍니다.
                overlayCanvas.alpha = Mathf.Clamp01(overlayCanvas.alpha - Time.deltaTime * 2.0f);

                if (overlayCanvas.alpha <= 0.0f)
                {
                    // 알파 값이 0 이하이면 실행되며, 오버레이를 숨기고 초기화합니다.
                    OverlayVisible = false;
                    m_Internal.Overlay.gameObject.SetActive(false);
                }
            }

            // 패스워드 창.
            if (m_Internal.PasswordDialog.gameObject.activeInHierarchy
                && m_Internal.PasswordInputField.text != ""
                && Input.GetKeyDown("enter"))
            {
                // 암호 대화상자가 활성화되어 있고, 입력 필드에 텍스트가 있으며, 엔터 키가 눌렸을 때 실행됩니다.
                OnPasswordDialogOkButtonClicked();
            }

            // 문서가 유효하고 로드되었을 때 실행됩니다.
            if (m_Document == null || !m_Document.IsValid || !m_IsLoaded)
                return;


            // 스크롤 바의 가시성을 업데이트합니다.
            UpdateScrollBarVisibility();

            // 페이지 컨테이너의 위치를 유효하게 조정합니다.
            EnsureValidPageContainerPosition();

            // 페이지 피팅이 이전 페이지 피팅과 다른 경우, 줌을 조정합니다.
            if (m_PageFitting != m_PreviousPageFitting)
                AdjustZoomToPageFitting(m_PageFitting, m_NormalPageSizes[0]);

            // 줌 팩터가 변경되었을 때 실행됩니다.
            if (Math.Abs(m_ZoomFactor - m_ZoomToGo) > 0.001f)
            {
                // 줌 팩터를 보간하여 부드럽게 조절합니다.
                m_ZoomToGo = Mathf.Clamp(m_ZoomToGo, m_MinZoomFactor, m_MaxZoomFactor);
                m_ZoomFactor = Mathf.Lerp(m_ZoomFactor, m_ZoomToGo, Time.deltaTime * 15.0f);

                m_UpdateChangeDelay = m_DelayAfterZoomingBeforeUpdate;
            }
            else
            {
                // 줌 팩터가 목표치에 도달하면 값을 설정합니다.
                m_ZoomFactor = m_ZoomToGo;
            }

            // 줌이 변경되었는지 확인합니다.
            bool zoomHasChanged = m_PreviousZoom != 0.0f && Math.Abs(m_PreviousZoom - m_ZoomFactor) > float.Epsilon;

            // 초기 상태
            if (m_PreviousZoom == 0.0f)
            {
                // 초기 상태에서는 스크롤 바와 페이지 컨테이너의 위치를 업데이트합니다.
                UpdateScrollBarVisibility();
                EnsureValidPageContainerPosition();
            }

            if (zoomHasChanged)
            {
                // 줌이 변경된 경우 실행됩니다.
                Vector2 oldDocumentSize = GetDocumentSize();

                ComputePageSizes();
                ComputePageOffsets();
                UpdatePagesPlacement();

                m_Internal.PageContainer.sizeDelta = GetDocumentSize();

                float newDocumentWidthRatio = m_Internal.PageContainer.sizeDelta.x / oldDocumentSize.x;
                float newDocumentHeightRatio = m_Internal.PageContainer.sizeDelta.y / oldDocumentSize.y;

                float deltaOffsetY = (m_Internal.PageContainer.anchoredPosition.y + m_ZoomPosition.y) *
                                     newDocumentHeightRatio - m_Internal.PageContainer.anchoredPosition.y - m_ZoomPosition.y;

                float deltaOffsetX = (m_Internal.PageContainer.anchoredPosition.x + m_ZoomPosition.x) *
                                     newDocumentWidthRatio - m_Internal.PageContainer.anchoredPosition.x - m_ZoomPosition.x;

                m_Internal.PageContainer.anchoredPosition += Vector2.up * deltaOffsetY + Vector2.right * deltaOffsetX;

                UpdateScrollBarVisibility();

                SetZoomLabel();
            }
            else if (Input.touchCount < 2)
            {
                // 터치 입력이 2개 미만인 경우 페이지 컨테이너의 위치를 유효하게 조정합니다.
                EnsureValidPageContainerPosition();
            }

            PDFPageRange oldPageRange = m_CurrentPageRange;

            // 현재 페이지 설정.
            m_CurrentPageRange = GetVisiblePageRange();

            if (!m_Internal.PageInputField.isFocused)
            {
                // 페이지 입력 필드에 포커스가 없는 경우 현재 페이지 번호를 업데이트합니다.
                int p = GetMostVisiblePageIndex() + 1;
                m_Internal.PageInputField.text = p.ToString();

                SetPageCountLabel(p, m_PageCount);
            }

            if (!oldPageRange.Equals(m_CurrentPageRange) && m_CurrentPageRange.IsValid
                || zoomHasChanged && m_CurrentPageRange.IsValid)
            {
                // 페이지 범위가 변경되었거나 줌이 변경되었을 때 페이지 텍스처를 업데이트합니다.
                float scale = Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality);

                PDFPageRange.UpdatePageAgainstRanges(oldPageRange, m_CurrentPageRange, m_Document, m_PageTextureHolders, m_RenderSettings, scale, this, m_NormalPageSizes);
            }

            int mostVisible = GetMostVisiblePageIndex();

            if (m_PreviousMostVisiblePage != mostVisible)
            {
                // 화면에 가장 많이 보이는 페이지가 변경된 경우 이를 알립니다.
                NotifyCurrentPageChanged(m_PreviousMostVisiblePage, mostVisible);

                m_PreviousMostVisiblePage = GetMostVisiblePageIndex();
            }

            if (!oldPageRange.Equals(m_CurrentPageRange))
            {
                // 페이지 범위가 변경된 경우 현재 검색 결과를 조정합니다.
                AdjustCurrentSearchResultDisplayed();
            }

            if (Math.Abs(m_PreviousZoomToGo - m_ZoomToGo) > float.Epsilon)
            {
                // 줌 토글이 변경된 경우 이를 알립니다.
                NotifyZoomChanged(m_PreviousZoomToGo, m_ZoomToGo);
            }

            if (m_UpdateChangeDelay != 0.0f && !zoomHasChanged)
            {
                m_UpdateChangeDelay -= Time.deltaTime;

                if (m_UpdateChangeDelay <= 0.0f)
                {
                    m_UpdateChangeDelay = 0.0f;

                    for (int i = m_CurrentPageRange.m_From; i < m_CurrentPageRange.m_To; ++i)
                    {
#if UNITY_WEBGL
            m_PageTextureHolders[i].Visible = true;

            if (m_PageTextureHolders[i].RenderingStarted)
                continue;

            int w = (int)(m_NormalPageSizes[i].x * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));
            int h = (int)(m_NormalPageSizes[i].y * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));

            m_PageTextureHolders[i].RenderingStarted = true;

            StartCoroutine(UpdatePageRangeTextures(i, w, h));
#else
                        if (m_PageTextureHolders[i].Texture != null)
                        {
                            // 이미 할당된 텍스처가 있으면 이전 텍스처 해제
                            Texture2D tex = m_PageTextureHolders[i].Texture;
                            m_PageTextureHolders[i].Texture = null;

                            Destroy(tex);
                        }

                        // 페이지 너비와 높이 계산
                        int w = (int)(m_Document.GetPageWidth(i) * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));
                        int h = (int)(m_Document.GetPageHeight(i) * Mathf.Min(m_ZoomToGo, m_MaxZoomFactorTextureQuality));

                        if (m_Renderer == null)
                            m_Renderer = new PDFRenderer();

                        using (PDFPage page = m_Document.GetPage(i))
                        {
                            // 페이지를 텍스처로 렌더링 한다.
                            Texture2D newTex = m_Renderer.RenderPageToTexture(page, w, h, this, m_RenderSettings);

                            // 새로 렌더링된 텍스처 할당.
                            m_PageTextureHolders[i].Texture = newTex;
                        }
#endif
                    }
                }
            }

            // 이전 줌 값 및 페이지 피팅 상태를 저장합니다.
            m_PreviousZoom = m_ZoomFactor;
            m_PreviousZoomToGo = m_ZoomToGo;
            m_PreviousPageFitting = m_PageFitting;

            // 썸네일 뷰어와 북마크 뷰어가 활성화되어 있다면 업데이트합니다.
            if (m_ThumbnailsViewer.gameObject.activeInHierarchy)
                m_ThumbnailsViewer.DoUpdate();
            if (m_BookmarksViewer.gameObject.activeInHierarchy)
                m_BookmarksViewer.DoUpdate();
        }

        // 페이지 텍스처 업데이트를 수행.
#if UNITY_WEBGL
        private IEnumerator UpdatePageRangeTextures(int pageIndex, int w, int h)
        {
            PDFJS_Promise<PDFPage> pagePromise = m_Document.GetPageAsync(pageIndex);

            while (!pagePromise.HasFinished)
                yield return null;

            if (pagePromise.HasSucceeded)
            {
                PDFJS_Promise<Texture2D> renderPromise = PDFRenderer.RenderPageToTextureAsync(pagePromise.Result, w, h);

                m_PageTextureHolders[pageIndex].RenderingPromise = renderPromise;

                while (!renderPromise.HasFinished)
                    yield return null;

                m_PageTextureHolders[pageIndex].RenderingPromise = null;
                m_PageTextureHolders[pageIndex].RenderingStarted = false;

                if (renderPromise.HasSucceeded)
                {
                    if (m_PageTextureHolders[pageIndex].Texture != null && m_PageTextureHolders[pageIndex].Texture != renderPromise.Result)
                    {
                        Destroy(m_PageTextureHolders[pageIndex].Texture);
                        m_PageTextureHolders[pageIndex].Texture = null;
                    }

                    if (m_PageTextureHolders[pageIndex].Visible)
                    {
                        m_PageTextureHolders[pageIndex].Texture = renderPromise.Result;
                    }
                    else
                    {
                        Destroy(renderPromise.Result);
                        renderPromise.Result = null;
                    }
                }
            }
            else
            {
                m_PageTextureHolders[pageIndex].RenderingPromise = null;
                m_PageTextureHolders[pageIndex].RenderingStarted = false;
            }
        }
#endif
        public Vector2[] GetCachedNormalPageSizes()
        {
            return m_NormalPageSizes;
        }

        // 페이지 크기 및 위치 업데이트를 수행.
        private void UpdatePagesPlacement()
        {
            //if (m_PageTextureHolders == null || m_PageSizes == null)
            //    return;

            ////원래 코드

            //foreach (PDFPageTextureHolder holder in m_PageTextureHolders)
            //{
            //    RectTransform holderRectTransform = (RectTransform)holder.Page.transform;

            //    holderRectTransform.sizeDelta = m_PageSizes[holder.PageIndex];
            //    holder.RefreshTexture();

            //    Vector3 newPosition = holderRectTransform.anchoredPosition3D;
            //    newPosition.x = 0;
            //    newPosition.y = -m_PageOffsets[holder.PageIndex];
            //    newPosition.z = 0;
            //    holderRectTransform.anchoredPosition3D = newPosition;
            //}
            Debug.Log("UpdatePagesPlacement() 함수");

            // 수정 코드 
            if (m_PageTextureHolders == null || m_PageSizes == null)
                return;

            for (int i = 0; i < m_PageCount; ++i)
            {
                PDFPageTextureHolder holder = m_PageTextureHolders[i];
                RectTransform holderRectTransform = (RectTransform)holder.Page.transform;

                holderRectTransform.sizeDelta = m_PageSizes[holder.PageIndex];
                holder.RefreshTexture();

                Vector3 newPosition = holderRectTransform.anchoredPosition3D;

                if (i % 2 == 0)
                {
                    // 왼쪽 페이지 위치 설정
                    newPosition.x = -m_PageSizes[i].x * 0.5f;
                }
                else
                {
                    // 오른쪽 페이지 위치 설정
                    newPosition.x = m_PageSizes[i - 1].x * 0.5f; // 왼쪽 페이지의 절반 너비만큼 오른쪽으로 이동
                }

                newPosition.y = -m_PageOffsets[holder.PageIndex];
                newPosition.z = 0;

                holderRectTransform.anchoredPosition3D = newPosition;
            }



        }

        // 스크롤바 가시성 업데이트를 수행.
        private void UpdateScrollBarVisibility()
        {
            bool vScrollVisible = true;
            bool hScrollVisible = true;

            if (Application.isPlaying)
            {
                Vector2 pageContainerSizeDelta = m_Internal.PageContainer.sizeDelta;
                Vector2 viewportRectSize = m_Internal.Viewport.rect.size;

                vScrollVisible = pageContainerSizeDelta.y > viewportRectSize.y;
                hScrollVisible = pageContainerSizeDelta.x > viewportRectSize.x;
            }

            vScrollVisible = vScrollVisible && m_ShowVerticalScrollBar;
            hScrollVisible = hScrollVisible && m_ShowHorizontalScrollBar;

            if (m_ViewportScrollRect == null)
                m_ViewportScrollRect = m_Internal.Viewport.GetComponent<ScrollRect>();

            if (m_HorizontalScrollBar == null)
                m_HorizontalScrollBar = m_Internal.HorizontalScrollBar.GetComponent<Scrollbar>();

            if (m_VerticalScrollBar == null)
                m_VerticalScrollBar = m_Internal.VerticalScrollBar.GetComponent<Scrollbar>();

            if (!hScrollVisible)
            {
                m_Internal.Viewport.offsetMin = new Vector2(m_Internal.Viewport.offsetMin.x, 0.0f);

                if (m_ViewportScrollRect.horizontalScrollbar != null)
                {
                    m_ViewportScrollRect.horizontalScrollbar = null;
                    m_Internal.HorizontalScrollBar.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Internal.Viewport.offsetMin = new Vector2(m_Internal.Viewport.offsetMin.x, 20.0f);

                if (m_ViewportScrollRect.horizontalScrollbar != m_HorizontalScrollBar)
                {
                    m_ViewportScrollRect.horizontalScrollbar = m_HorizontalScrollBar;
                    m_Internal.HorizontalScrollBar.gameObject.SetActive(true);
                }
            }

            if (!vScrollVisible)
            {
                m_Internal.Viewport.offsetMax = new Vector2(0.0f, m_Internal.Viewport.offsetMax.y);

                if (m_ViewportScrollRect.verticalScrollbar != null)
                {
                    m_ViewportScrollRect.verticalScrollbar = null;
                    m_Internal.VerticalScrollBar.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Internal.Viewport.offsetMax = new Vector2(-20.0f, m_Internal.Viewport.offsetMax.y);

                if (m_ViewportScrollRect.verticalScrollbar != m_VerticalScrollBar)
                {
                    m_ViewportScrollRect.verticalScrollbar = m_VerticalScrollBar;
                    m_Internal.VerticalScrollBar.gameObject.SetActive(true);
                }
            }

            if (hScrollVisible && vScrollVisible)
            {
                m_Internal.VerticalScrollBar.offsetMin = new Vector2(m_Internal.VerticalScrollBar.offsetMin.x, 19.0f);
                m_Internal.HorizontalScrollBar.offsetMax = new Vector2(-19.0f, m_Internal.HorizontalScrollBar.offsetMax.y);

                if (!m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(true);
            }
            else if (!hScrollVisible)
            {
                m_Internal.VerticalScrollBar.offsetMin = new Vector2(m_Internal.VerticalScrollBar.offsetMin.x, 0.0f);

                if (m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(false);
            }
            else
            {
                m_Internal.HorizontalScrollBar.offsetMax = new Vector2(0.0f, m_Internal.HorizontalScrollBar.offsetMax.y);

                if (m_Internal.ScrollCorner.gameObject.activeInHierarchy)
                    m_Internal.ScrollCorner.gameObject.SetActive(false);
            }
        }

        // 줌을 조정하는 함수,
        private void ZoomCommon(Vector2 zoomPosition, bool zoomIn, bool useSpecificZoom = false, float specificZoom = 1.0f)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

            m_ZoomPosition = zoomPosition;

            if (useSpecificZoom)
            {
                m_ZoomToGo = specificZoom;
            }
            else
            {
                float step = m_ZoomStep;

                if (m_ZoomToGo >= 2.0f)
                    step *= 2.0f;

                if (m_ZoomToGo >= 4.0f)
                    step *= 2.0f;

                float epsilon = m_ZoomStep * 0.125f;

                if (zoomIn)
                {
                    if (!Mathf.Approximately(Mathf.Floor(m_ZoomToGo * (1 / step)), m_ZoomToGo * (1 / step))
                        && m_ZoomToGo * (1 / step) <= Mathf.Floor(m_ZoomToGo * (1 / step)))
                    {
                        m_ZoomToGo = Mathf.Floor(m_ZoomToGo * (1 / step)) * step;
                    }

                    m_ZoomToGo = Mathf.Clamp(Mathf.Floor((m_ZoomToGo + step) * (1 / step) + epsilon) * step, m_MinZoomFactor, m_MaxZoomFactor);
                }
                else
                {
                    if (!Mathf.Approximately(Mathf.Floor(m_ZoomToGo * (1 / step)), m_ZoomToGo * (1 / step))
                        && m_ZoomToGo * (1 / step) >= Mathf.Floor(m_ZoomToGo * (1 / step)))
                    {
                        m_ZoomToGo = Mathf.Floor(m_ZoomToGo * (1 / step)) * step;
                    }

                    m_ZoomToGo = Mathf.Clamp(Mathf.Floor((m_ZoomToGo - step) * (1 / step) + epsilon) * step, m_MinZoomFactor, m_MaxZoomFactor);
                }
            }

            m_PageFitting = PageFittingType.Zoom;

            // 수정 코드

        }

        // 페이지 인덱스에 해당하는 장치의 페이지 크기를 반환
        public Vector2 GetDevicePageSize(int pageIndex)
        {
            return m_PageSizes[pageIndex];
        }

        // 북마크 관련 동작 핸들러를 반환합니다.
        public IPDFDeviceActionHandler GetBookmarksActionHandler()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return null;
#else
            return m_BookmarksActionHandler;
#endif
        }

        // 링크 관련 동작 핸들러를 반환,
        public IPDFDeviceActionHandler GetLinksActionHandler()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return null;
#else
            return m_LinksActionHandler;
#endif
        }

        // 페이지의 배경에서 색이 칠해진 사각형 목록을 반환.
        public IList<PDFColoredRect> GetBackgroundColoredRectList(PDFPage page)
        {
#if !UNITY_WEBGL
            if (m_SearchResults != null && m_SearchResults[page.PageIndex] != null &&
                m_SearchResults[page.PageIndex].Count > 0)
            {
                using (PDFTextPage textPage = page.GetTextPage())
                {
                    List<PDFColoredRect> coloredRectList = new List<PDFColoredRect>();

                    foreach (PDFSearchResult result in m_SearchResults[page.PageIndex])
                    {
                        int pageRectCount = textPage.CountRects(result.StartIndex, result.Count);

                        for (int j = 0; j < pageRectCount; ++j)
                        {
                            Rect rect = textPage.GetRect(j);
                            rect = new Rect(
                                rect.xMin - m_SearchResultPadding.x,
                                rect.yMin + m_SearchResultPadding.y,
                                rect.width + 2 * m_SearchResultPadding.x,
                                rect.height + 2 * m_SearchResultPadding.y);
                            coloredRectList.Add(new PDFColoredRect(rect, m_SearchResultColor));
                        }
                    }

                    return coloredRectList;
                }
            }
#endif

            return null;
        }

        // 문단을 확대.
        public void ZoomOnParagraph(PDFViewerPage viewerPage, Rect pageRect)
        {
            if (m_Document == null || !m_Document.IsValid)
                return;

#if !UNITY_WEBGL
            Vector3[] pageCorners = new Vector3[4];

            RectTransform viewerPageTransform = (RectTransform)viewerPage.transform;

            viewerPageTransform.GetWorldCorners(pageCorners);
            Vector2 min = pageCorners[0];
            Vector2 max = pageCorners[0];
            for (int i = 1; i < 4; ++i)
            {
                if (pageCorners[i].x < min.x)
                    min.x = pageCorners[i].x;
                if (pageCorners[i].y < min.y)
                    min.y = pageCorners[i].y;
                if (pageCorners[i].x > max.x)
                    max.x = pageCorners[i].x;
                if (pageCorners[i].y > max.y)
                    max.y = pageCorners[i].y;
            }

            Vector2 devicePageSize = viewerPageTransform.sizeDelta;

            using (PDFPage page = m_Document.GetPage(viewerPage.PageIndex))
            {
                Rect deviceRect = page.ConvertPageRectToDeviceRect(pageRect, devicePageSize);

                float deviceRectCenterPosition = deviceRect.max.y + (deviceRect.min - deviceRect.max).y * 0.5f;

                m_Internal.PageContainer.anchoredPosition = new Vector2(
                    m_Internal.PageContainer.anchoredPosition.x,
                    m_PageOffsets[page.PageIndex]
                    - m_PageSizes[page.PageIndex].y * 0.5f
                    + (m_PageSizes[page.PageIndex].y - deviceRectCenterPosition)
                    - m_Internal.Viewport.rect.size.y * 0.5f);
            }

            if (m_ZoomToGo < m_ParagraphZoomFactor)
                ZoomCommon(new Vector2(0.0f, m_Internal.Viewport.rect.size.y * 0.5f), true, true, m_ParagraphZoomFactor);
#endif
        }

        //부모 변환이 변경될 때 호출
        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();

            if (gameObject != null)
                m_Canvas = null;
        }

        // 캔버스 캐시
        private void CacheCanvas()
        {
            if (gameObject == null)
                return;

            gameObject.GetComponentsInParent(false, m_CanvasList);

            if (m_CanvasList.Count > 0)
            {
                // Find the first active and enabled canvas.
                for (int i = 0; i < m_CanvasList.Count; ++i)
                {
                    if (m_CanvasList[i].isActiveAndEnabled)
                    {
                        m_Canvas = m_CanvasList[i];
                        m_GraphicRaycaster = m_Canvas.GetComponent<GraphicRaycaster>();
                        break;
                    }
                }
            }
            else
            {
                m_Canvas = null;
                m_GraphicRaycaster = null;
            }

            m_CanvasList.Clear();
        }
    }
}