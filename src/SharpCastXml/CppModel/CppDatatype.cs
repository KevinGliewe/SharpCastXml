using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel {
    public class CppDatatype : CppElement {
        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        /// <value>The align.</value>
        [XmlAttribute("align")]
        public int Align { get; set; }

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        /// <value>The align.</value>
        [XmlAttribute("size")]
        public int Size { get; set; }
    }
}
