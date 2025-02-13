using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel.Traits
{
    public interface IContextTrait
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
