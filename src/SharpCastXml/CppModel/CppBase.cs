using SharpCastXml.CppModel.Traits;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel
{
    [XmlType("base")]
    public class CppBase: IAccessTrait
    {
        /// <summary>
        /// Raw offset in struct in bits
        /// </summary>
        [XmlAttribute("offset")]
        public int Offset { get; set; }

        public bool Virtual { get; set; }

        public CppAccess Access { get; set; }

        [XmlAttribute("basetype")]
        public CppStruct BaseType { get; set; }
    }
}
