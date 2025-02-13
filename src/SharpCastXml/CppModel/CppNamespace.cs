using SharpCastXml.CppModel.Traits;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel
{
    [XmlType("namespace")]
    public class CppNamespace : CppElement, IContextTrait
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The Context.
        /// </value>
        [XmlElement("context")]
        public CppElement Context { get; set; }
    }
}
