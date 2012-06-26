using System;
using System.Xml.Serialization;

namespace BusinessObject
{
    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "rsp")]
    public class VimeoErrorResponse
    {
        [XmlElementAttribute("err", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public VimeoErrorWrapper error { get; set; }
        [XmlAttributeAttribute()]
        public string generated_in { get; set; }
        [XmlAttributeAttribute()]
        public string stat { get; set; }
    }

    [SerializableAttribute]
    [XmlTypeAttribute(AnonymousType = true)]
    public class VimeoErrorWrapper
    {
        [XmlAttributeAttribute()]
        public string code { get; set; }
        [XmlAttributeAttribute()]
        public string msg { get; set; }
    }
}
