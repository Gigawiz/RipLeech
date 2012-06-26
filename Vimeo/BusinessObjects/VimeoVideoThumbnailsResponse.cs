using System;
using System.Xml.Serialization;

namespace BusinessObject
{
    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "rsp")]
    public class VimeoVideoThumbnailsResponse
    {
        [XmlElementAttribute("thumbnails", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public VimeoVideoThumbnailsWrapper thumbnails { get; set; }
        [XmlAttributeAttribute()]
        public string generated_in { get; set; }
        [XmlAttributeAttribute()]
        public string stat { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    public class VimeoVideoThumbnailsWrapper
    {
        [XmlElementAttribute("thumbnail", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public VimeoVideoThumbnailWrapper[] thumbnail { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    public class VimeoVideoThumbnailWrapper
    {
        [XmlAttributeAttribute()]
        public string height { get; set; }
        [XmlAttributeAttribute()]
        public string width { get; set; }
        [XmlText()]
        public string thumbnail { get; set; }
    }
}
