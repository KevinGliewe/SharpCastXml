// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace SharpCastXml.CppModel
{
    /// <summary>
    /// A C++ define macro Name=Value.
    /// </summary>
    [XmlType("unimplemented")]
    public class CppUnimplemented : CppDatatype
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CppUnimplemented"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage()]
        public CppUnimplemented()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CppUnimplemented"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public CppUnimplemented(string kind, string type_class)
        {
            Kind = kind;
            TypeClass = type_class;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The Kind.</value>
        [XmlAttribute("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The TypeClass.</value>
        [XmlAttribute("type_class")]
        public string TypeClass { get; set; }
    }
}