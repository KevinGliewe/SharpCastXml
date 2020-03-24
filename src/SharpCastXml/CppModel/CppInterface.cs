﻿// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel
{
    /// <summary>
    /// A C++ interface.
    /// </summary>
    [XmlType("interface")]
    public class CppInterface : CppElement
    {
        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        [XmlAttribute("guid")]
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>The name of the parent.</value>
        [XmlAttribute("base")]
        public string Base { get; set; }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>The methods.</value>
        [XmlIgnore]
        public IEnumerable<CppMethod> Methods
        {
            get { return Iterate<CppMethod>(); }
        }

        /// <summary>
        /// Gets or sets the total method count.
        /// </summary>
        /// <value>The total method count.</value>
        [XmlIgnore]
        public int TotalMethodCount { get; set; }
    }
}