using System;
using System.Xml.Serialization;

namespace BusinessObject
{
    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "rsp")]
    public partial class VimeoSearchResponse
    {
        [XmlElementAttribute("videos", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResponseVideosWrapper videos { get; set; }
        [XmlAttributeAttribute()]
        public string generated_in { get; set; }
        [XmlAttributeAttribute()]
        public string stat { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class SearchResponseVideosWrapper
    {
        [XmlElementAttribute("video", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResponseVideosWrapperVideo[] video { get; set; }
        [XmlAttributeAttribute()]
        public string on_this_page { get; set; }
        [XmlAttributeAttribute()]
        public string page { get; set; }
        [XmlAttributeAttribute()]
        public string perpage { get; set; }
        [XmlAttributeAttribute()]
        public string total { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class SearchResponseVideosWrapperVideo
    {
        [XmlAttributeAttribute()]
        public string embed_privacy { get; set; }
        [XmlAttributeAttribute()]
        public string id { get; set; }
        [XmlAttributeAttribute()]
        public string is_hd { get; set; }
        [XmlAttributeAttribute()]
        public string owner { get; set; }
        [XmlAttributeAttribute()]
        public string privacy { get; set; }
        [XmlAttributeAttribute()]
        public string title { get; set; }
        [XmlAttributeAttribute()]
        public string upload_date { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VimeoSearchResponses
    {
        [XmlElementAttribute("rsp")]
        public VimeoSearchResponse[] items { get; set; }
    }
}
