using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.DocumentSee.LIB.Proto;
using XTC.FMP.MOD.DocumentSee.LIB.MVCS;
using Paroxe.PdfRenderer;
using Paroxe.PdfRenderer.Internal.Viewer;

namespace XTC.FMP.MOD.DocumentSee.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        public class UiReference
        {
            public Image background;
            public GameObject loading;
            public GameObject btnSearch;
            public Transform toolbar;
            public Text txtError;
            public PDFViewer pdfViewer;
            public PDFViewerInternal pdfViewerInternal;
            public PDFViewerLeftPanel pdfViewerLeftPanel;
            public PDFThumbnailsViewer pdfThumbnailsViewer;
            public PDFThumbnailItem pdfThumbnailItem;
            public PDFBookmarksViewer pdfBookmarksViewer;
            public PDFBookmarkListItem pdfBookmarkListItem;
            public PDFSearchPanel pdfSearchPanel;
            public PDFViewerPage pdfViewerPage;
            public PDFViewerSearchButton pdfViewerSearchButton;
            public PDFViewerLeftPanelScrollbar pdfViewerLeftPanelScrollbarBookmark;
            public PDFViewerLeftPanelScrollbar pdfViewerLeftPanelScrollbarThumbnail;
        }

        private UiReference uiReference_ = new UiReference();

        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {
            uiReference_.background = rootUI.transform.Find("bg").GetComponent<Image>();
            uiReference_.background.gameObject.SetActive(style_.background.visible);
            uiReference_.loading = rootUI.transform.Find("loading").gameObject;
            uiReference_.btnSearch = rootUI.transform.Find("PDFViewer/Internal/TopPanel/SearchButton").gameObject;
            uiReference_.btnSearch.SetActive(false);
            uiReference_.txtError = rootUI.transform.Find("loading/txtError").GetComponent<Text>();
            uiReference_.toolbar = rootUI.transform.Find("PDFViewer/Internal/TopPanel");
            uiReference_.toolbar.gameObject.SetActive(style_.toolbar.visible);


            // search
            uiReference_.pdfSearchPanel = rootUI.transform.Find("PDFViewer/Internal/SearchPanel").gameObject.AddComponent<PDFSearchPanel>();
            uiReference_.pdfSearchPanel.m_ContentPanel = uiReference_.pdfSearchPanel.transform.Find("ContentPanel").GetComponent<RectTransform>();
            uiReference_.pdfSearchPanel.m_InputField = uiReference_.pdfSearchPanel.transform.Find("ContentPanel/InputField").GetComponent<InputField>();
            uiReference_.pdfSearchPanel.m_MatchCaseCheckBox = uiReference_.pdfSearchPanel.transform.Find("ContentPanel/MatchCaseCheckBox/CheckMark").GetComponent<Image>();
            uiReference_.pdfSearchPanel.m_MatchWholeWordCheckBox = uiReference_.pdfSearchPanel.transform.Find("ContentPanel/MatchWholeWordCheckBox/CheckMark").GetComponent<Image>();
            uiReference_.pdfSearchPanel.m_TotalResultText = uiReference_.pdfSearchPanel.transform.Find("ContentPanel/GameObject/Validator/Results").GetComponent<Text>();
            uiReference_.pdfSearchPanel.m_ValidatorImage = uiReference_.pdfSearchPanel.transform.Find("ContentPanel/GameObject/Validator").GetComponent<Image>();
            uiReference_.pdfViewerSearchButton = rootUI.transform.Find("PDFViewer/Internal/TopPanel/SearchButton/SearchButton").gameObject.AddComponent<PDFViewerSearchButton>();

            // BookmarkItem
            uiReference_.pdfBookmarkListItem = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Bookmarks/BookmarkContainer/BookmarkItem").gameObject.AddComponent<PDFBookmarkListItem>();
            uiReference_.pdfBookmarkListItem.m_CollapseSprite = rootAttachments.transform.Find("ListCollapseSprite").GetComponent<Image>().sprite;
            uiReference_.pdfBookmarkListItem.m_ExpandImage = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/Expand").GetComponent<Image>();
            uiReference_.pdfBookmarkListItem.m_ExpandSprite = rootAttachments.transform.Find("ListExpandSprite").GetComponent<Image>().sprite;
            uiReference_.pdfBookmarkListItem.m_Highlighted = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/Highlighted").GetComponent<Image>();
            uiReference_.pdfBookmarkListItem.m_HorizontalLine = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/HorizontalLine").GetComponent<RectTransform>();
            uiReference_.pdfBookmarkListItem.m_Internal = uiReference_.pdfBookmarkListItem.transform.Find("_Internal").GetComponent<RectTransform>();
            uiReference_.pdfBookmarkListItem.m_Title = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/Title").GetComponent<Text>();
            uiReference_.pdfBookmarkListItem.m_VerticalLine = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/VerticalLine").GetComponent<RectTransform>();
            uiReference_.pdfBookmarkListItem.m_VerticalLine2 = uiReference_.pdfBookmarkListItem.transform.Find("_Internal/VerticalLine2").GetComponent<RectTransform>();

            // BookmarkViewer
            uiReference_.pdfBookmarksViewer = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Bookmarks").gameObject.AddComponent<PDFBookmarksViewer>();
            uiReference_.pdfBookmarksViewer.m_BooksmarksContainer = uiReference_.pdfBookmarksViewer.transform.Find("BookmarkContainer").GetComponent<RectTransform>();
            uiReference_.pdfBookmarksViewer.m_ItemPrefab = uiReference_.pdfBookmarkListItem;
            uiReference_.pdfViewerLeftPanelScrollbarBookmark = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Bookmarks/VerticalScrollbar").gameObject.AddComponent<PDFViewerLeftPanelScrollbar>();

            // ThumbnailItem
            uiReference_.pdfThumbnailItem = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Thumbnails/ThumbnailsContainer/ThumbnailItem").gameObject.AddComponent<PDFThumbnailItem>();
            uiReference_.pdfThumbnailItem.m_AspectRatioFitter = uiReference_.pdfThumbnailItem.transform.Find("ThumbailZone/Thumbnail").GetComponent<AspectRatioFitter>();
            uiReference_.pdfThumbnailItem.m_Highlighted = uiReference_.pdfThumbnailItem.transform.Find("Highlighted").GetComponent<Image>();
            uiReference_.pdfThumbnailItem.m_LayoutElement = uiReference_.pdfThumbnailItem.transform.GetComponent<LayoutElement>();
            uiReference_.pdfThumbnailItem.m_PageIndexLabel = uiReference_.pdfThumbnailItem.transform.Find("PageIndex").GetComponent<Text>();
            uiReference_.pdfThumbnailItem.m_PageThumbnailRawImage = uiReference_.pdfThumbnailItem.transform.Find("ThumbailZone/Thumbnail").GetComponent<RawImage>();
            uiReference_.pdfThumbnailItem.m_RectTransform = uiReference_.pdfThumbnailItem.transform.GetComponent<RectTransform>();

            // ThumbnailViewer
            uiReference_.pdfThumbnailsViewer = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Thumbnails").gameObject.AddComponent<PDFThumbnailsViewer>();
            uiReference_.pdfThumbnailsViewer.m_ThumbnailItemPrefab = uiReference_.pdfThumbnailItem;
            uiReference_.pdfThumbnailsViewer.m_ThumbnailsContainer = uiReference_.pdfThumbnailsViewer.transform.Find("ThumbnailsContainer").GetComponent<RectTransform>();
            uiReference_.pdfViewerLeftPanelScrollbarThumbnail = rootUI.transform.Find("PDFViewer/Internal/LeftPanel/Thumbnails/VerticalScrollbar").gameObject.AddComponent<PDFViewerLeftPanelScrollbar>();

            // LeftPanel
            uiReference_.pdfViewerLeftPanel = rootUI.transform.Find("PDFViewer/Internal/LeftPanel").gameObject.AddComponent<PDFViewerLeftPanel>();
            uiReference_.pdfViewerLeftPanel.m_Bookmarks = uiReference_.pdfViewerLeftPanel.transform.Find("Bookmarks").GetComponent<RectTransform>();
            uiReference_.pdfViewerLeftPanel.m_BookmarksTab = uiReference_.pdfViewerLeftPanel.transform.Find("Tabs/Bookmarks").GetComponent<Image>();
            uiReference_.pdfViewerLeftPanel.m_BookmarksTabTitle = uiReference_.pdfViewerLeftPanel.transform.Find("Tabs/Bookmarks/Text").GetComponent<Text>();
            uiReference_.pdfViewerLeftPanel.m_ClosedTabSprite = rootAttachments.transform.Find("ClosedTabSprite").GetComponent<Image>().sprite;
            uiReference_.pdfViewerLeftPanel.m_CloseSprite = rootAttachments.transform.Find("LeftPanelCloseSprite").GetComponent<Image>().sprite;
            uiReference_.pdfViewerLeftPanel.m_MaxWidth = 500;
            uiReference_.pdfViewerLeftPanel.m_MinWidth = 250;
            uiReference_.pdfViewerLeftPanel.m_OpenedTabSprite = rootAttachments.transform.Find("OpenedTabSprite").GetComponent<Image>().sprite;
            uiReference_.pdfViewerLeftPanel.m_OpenSprite = rootAttachments.transform.Find("LeftPanelOpenSprite").GetComponent<Image>().sprite;
            uiReference_.pdfViewerLeftPanel.m_ResizeCursor = rootAttachments.transform.Find("ResizeCursorTexture").GetComponent<RawImage>().texture as Texture2D;
            uiReference_.pdfViewerLeftPanel.m_SideBarImage = uiReference_.pdfViewerLeftPanel.transform.Find("SideBar/Arrow").GetComponent<Image>();
            uiReference_.pdfViewerLeftPanel.m_Thumbnails = uiReference_.pdfViewerLeftPanel.transform.Find("Thumbnails").GetComponent<RectTransform>();
            uiReference_.pdfViewerLeftPanel.m_ThumbnailsTab = uiReference_.pdfViewerLeftPanel.transform.Find("Tabs/Thumbnails").GetComponent<Image>();
            uiReference_.pdfViewerLeftPanel.m_ThumbnailsTabTitle = uiReference_.pdfViewerLeftPanel.transform.Find("Tabs/Thumbnails/Text").GetComponent<Text>();
            uiReference_.pdfViewerLeftPanel.m_ThumbnailsViewer = uiReference_.pdfThumbnailsViewer;

            // ViewerPage
            uiReference_.pdfViewerPage = rootUI.transform.Find("PDFViewer/Internal/Viewport/PageContainer/Page").gameObject.AddComponent<PDFViewerPage>();
            uiReference_.pdfViewerPage.m_HandCursor = rootAttachments.transform.Find("HandLinkCursorTexture").GetComponent<RawImage>().texture as Texture2D;

            // Viewer
            uiReference_.pdfViewer = rootUI.transform.Find("PDFViewer").gameObject.AddComponent<PDFViewer>();
            uiReference_.pdfViewer.m_AllowOpenURL = true;
            uiReference_.pdfViewer.m_ChangeCursorWhenOverURL = true;
            uiReference_.pdfViewer.m_LoadOnEnable = false;
            uiReference_.pdfViewer.m_MaxZoomFactor = 8;
            uiReference_.pdfViewer.m_MaxZoomFactorTextureQuality = 4;
            uiReference_.pdfViewer.m_MinZoomFactor = 0.25f;
            uiReference_.pdfViewer.m_PageFitting = PDFViewer.PageFittingType.WholePage;
            uiReference_.pdfViewer.m_ZoomFactor = 1;
            uiReference_.pdfViewer.m_ZoomStep = 0.25f;
            uiReference_.pdfViewer.m_VerticalMarginBetweenPages = 20;
            uiReference_.pdfViewer.m_UnloadOnDisable = false;
            uiReference_.pdfViewer.m_ShowVerticalScrollBar = false;
            uiReference_.pdfViewer.m_ShowBookmarksViewer = false;
            uiReference_.pdfViewer.m_ShowHorizontalScrollBar = false;
            uiReference_.pdfViewer.m_ShowThumbnailsViewer = false;
            uiReference_.pdfViewer.m_ShowTopBar = false;
            uiReference_.pdfViewer.m_ScrollSensitivity = 75;


            // ViewerInternal 
            uiReference_.pdfViewerInternal = rootUI.transform.Find("PDFViewer/Internal").gameObject.AddComponent<PDFViewerInternal>();
            uiReference_.pdfViewerInternal.m_PDFViewer = uiReference_.pdfViewer;
            uiReference_.pdfViewerInternal.m_DownloadDialog = uiReference_.pdfViewerInternal.transform.Find("DownloadDialog").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_DownloadSourceLabel = uiReference_.pdfViewerInternal.transform.Find("DownloadDialog/SourceLabelContainer/SourceLabel").GetComponent<Text>();
            uiReference_.pdfViewerInternal.m_HorizontalScrollBar = uiReference_.pdfViewerInternal.transform.Find("HorizontalScrollbar").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_InvalidPasswordImage = uiReference_.pdfViewerInternal.transform.Find("PasswordDialog/InvalidPassword").GetComponent<Image>();
            uiReference_.pdfViewerInternal.m_LeftPanel = uiReference_.pdfViewerLeftPanel;
            uiReference_.pdfViewerInternal.m_Overlay = uiReference_.pdfViewerInternal.transform.Find("Overlay").GetComponent<CanvasGroup>();
            uiReference_.pdfViewerInternal.m_PageContainer = uiReference_.pdfViewerInternal.transform.Find("Viewport/PageContainer").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_PageCountLabel = uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageCountLabel").GetComponent<Text>();
            uiReference_.pdfViewerInternal.m_PageDownButton = uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageDownButton").GetComponent<Button>();
            uiReference_.pdfViewerInternal.m_PageInputField = uiReference_.pdfViewerInternal.transform.Find("TopPanel/InputField").GetComponent<InputField>();
            uiReference_.pdfViewerInternal.m_PageSample = uiReference_.pdfViewerInternal.transform.Find("Viewport/PageContainer/Page").GetComponent<RawImage>();
            uiReference_.pdfViewerInternal.m_PageUpButton = uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageUpButton").GetComponent<Button>();
            uiReference_.pdfViewerInternal.m_PageZoomLabel = uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageZoomLabel").GetComponent<Text>();
            uiReference_.pdfViewerInternal.m_PasswordDialog = uiReference_.pdfViewerInternal.transform.Find("PasswordDialog").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_PasswordInputField = uiReference_.pdfViewerInternal.transform.Find("PasswordDialog/PasswordField").GetComponent<InputField>();
            uiReference_.pdfViewerInternal.m_ProgressLabel = uiReference_.pdfViewerInternal.transform.Find("DownloadDialog/ProgressBar/ProgressLabel").GetComponent<Text>();
            uiReference_.pdfViewerInternal.m_ProgressRect = uiReference_.pdfViewerInternal.transform.Find("DownloadDialog/ProgressBar/ProgressRect").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_ScrollCorner = uiReference_.pdfViewerInternal.transform.Find("ScrollBarCorner").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_ScrollRect = uiReference_.pdfViewerInternal.transform.Find("Viewport").GetComponent<ScrollRect>();
            uiReference_.pdfViewerInternal.m_TopPanel = uiReference_.pdfViewerInternal.transform.Find("TopPanel").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_VerticalScrollBar = uiReference_.pdfViewerInternal.transform.Find("VerticalScrollbar").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_Viewport = uiReference_.pdfViewerInternal.transform.Find("Viewport").GetComponent<RectTransform>();
            uiReference_.pdfViewerInternal.m_SearchPanel = uiReference_.pdfViewerInternal.transform.Find("SearchPanel").GetComponent<RectTransform>();

            uiReference_.pdfViewer.m_Internal = uiReference_.pdfViewerInternal;

            applyStyle();
            bindEvents();
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            rootUI.gameObject.SetActive(true);
            rootWorld.gameObject.SetActive(true);

            uiReference_.pdfViewer.gameObject.SetActive(false);
            uiReference_.loading.SetActive(true);
            uiReference_.txtError.gameObject.SetActive(false);

            uiReference_.pdfViewer.FileSource = PDFViewer.FileSourceType.FilePath;
            uiReference_.pdfViewer.FilePath = _uri;
            if (_source == "assloud://")
            {
                uiReference_.pdfViewer.FilePath = Path.Combine(settings_["path.assets"].AsString(), _uri);
            }
            uiReference_.pdfViewer.gameObject.SetActive(true);
            uiReference_.loading.SetActive(false);
            //必须在gameObject可见时调用
            uiReference_.pdfViewer.LoadDocument();
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            //必须在gameObject可见时调用
            uiReference_.pdfViewer.UnloadDocument();
            uiReference_.pdfViewer.gameObject.SetActive(false);
            rootUI.gameObject.SetActive(false);
            rootWorld.gameObject.SetActive(false);
        }


        private void applyStyle()
        {
            Func<string, Color> convertColor = (_color) =>
            {
                Color color = Color.white;
                if (!ColorUtility.TryParseHtmlString(_color, out color))
                    color = Color.black;
                return color;
            };
            uiReference_.background.color = convertColor(style_.background.color);
            uiReference_.loading.GetComponent<Image>().color = convertColor(style_.background.color);
            uiReference_.loading.transform.Find("imgPending").GetComponent<Image>().color = convertColor(style_.primaryColor);

            alignByAncor(uiReference_.toolbar, style_.toolbar.anchor);
        }

        private void bindEvents()
        {
            uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageUpButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.pdfViewerInternal.OnPreviousPageButtonClicked();
            });
            uiReference_.pdfViewerInternal.transform.Find("TopPanel/PageDownButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.pdfViewerInternal.OnNextPageButtonClicked();
            });
            uiReference_.pdfViewerInternal.transform.Find("TopPanel/ZoomOutButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.pdfViewerInternal.OnZoomOutButtonClicked();
            });
            uiReference_.pdfViewerInternal.transform.Find("TopPanel/ZoomInButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                uiReference_.pdfViewerInternal.OnZoomInButtonClicked();
            });
        }

    }
}
