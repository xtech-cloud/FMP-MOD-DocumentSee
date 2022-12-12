using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.DocumentSee.LIB.Proto;
using XTC.FMP.MOD.DocumentSee.LIB.MVCS;
using Paroxe.PdfRenderer;
using System.IO;

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
            public PDFViewer viewer;
            public GameObject btnSearch;
            public Transform toolbar;
            public Text txtError;
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
            uiReference_.viewer = rootUI.transform.Find("PDFViewer").GetComponent<PDFViewer>();
            uiReference_.btnSearch = rootUI.transform.Find("PDFViewer/Internal/TopPanel/SearchButton").gameObject;
            uiReference_.btnSearch.SetActive(false);
            uiReference_.txtError = rootUI.transform.Find("loading/txtError").GetComponent<Text>();
            uiReference_.toolbar = rootUI.transform.Find("PDFViewer/Internal/TopPanel");
            uiReference_.toolbar.gameObject.SetActive(style_.toolbar.visible);

            applyStyle();
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

            uiReference_.viewer.gameObject.SetActive(false);
            uiReference_.loading.SetActive(true);
            uiReference_.txtError.gameObject.SetActive(false);

            uiReference_.viewer.FileSource = PDFViewer.FileSourceType.FilePath;
            uiReference_.viewer.FilePath = _uri;
            if (_source == "assloud://")
            {
                uiReference_.viewer.FilePath = Path.Combine(settings_["path.assets"].AsString(), _uri);
            }
            uiReference_.viewer.gameObject.SetActive(true);
            uiReference_.loading.SetActive(false);
            //必须在gameObject可见时调用
            uiReference_.viewer.LoadDocument();
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            //必须在gameObject可见时调用
            uiReference_.viewer.UnloadDocument();
            uiReference_.viewer.gameObject.SetActive(false);
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

    }
}
