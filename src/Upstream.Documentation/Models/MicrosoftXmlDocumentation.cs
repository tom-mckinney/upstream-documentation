using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Upstream.Documentation.Models
{
    [XmlRoot("doc")]
    public class MicrosoftXmlDocumentation
    {
        [XmlArray("members")]
        [XmlArrayItem("member")]
        public Member[] Members { get; set; }

        //[XmlRoot("member")]
        public class Member
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlElement("summary")]
            public string Summary { get; set; }
        }
    }
}
