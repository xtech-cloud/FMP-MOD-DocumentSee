
using System.Xml.Serialization;

namespace XTC.FMP.MOD.DocumentSee.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class Background
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = true;
            [XmlAttribute("color")]
            public string color { get; set; } = "#000000FF";
        }

        public class ToolBar
        {
            [XmlAttribute("visible")]
            public bool visible { get; set; } = true;
            [XmlElement("Anchor")]
            public Anchor anchor { get; set; } = new Anchor();
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlAttribute("primaryColor")]
            public string primaryColor { get; set; } = "#FFFFFFFF";

            [XmlElement("Background")]
            public Background background { get; set; } = new Background();
            [XmlElement("ToolBar")]
            public ToolBar toolbar { get; set; } = new ToolBar();
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

