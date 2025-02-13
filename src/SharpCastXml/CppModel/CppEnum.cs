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
using SharpCastXml.CppModel.Traits;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel
{
    /// <summary>
    /// A C++ enum.
    /// </summary>
    [XmlType("enum")]
    public class CppEnum : CppElement, IContextTrait
    {
        /// <summary>
        /// Adds an enum item to this enum.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddEnumItem(string name, string value)
        {
            Add(new CppEnumItem(name, value));
        }

        /// <summary>
        /// Gets the enum items.
        /// </summary>
        /// <value>The enum items.</value>
        [XmlIgnore]
        public IEnumerable<CppEnumItem> EnumItems
        {
            get { return Iterate<CppEnumItem>(); }
        }


        [XmlAttribute("underlying-type")]
        public string UnderlyingType { get; set; } = "int";

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