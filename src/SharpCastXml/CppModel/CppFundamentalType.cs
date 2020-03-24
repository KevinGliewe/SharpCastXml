using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel {
    public class CppFundamentalType : CppDatatype {

        [XmlAttribute("isunsigned")]
        public Boolean IsUnsigned { get; set; }

        [XmlAttribute("shortcount")]
        public int ShortCount { get; set; }

        [XmlAttribute("longcount")]
        public int LongCount { get; set; }
    }
}
