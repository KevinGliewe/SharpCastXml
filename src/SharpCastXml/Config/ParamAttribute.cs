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
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SharpCastXml.Config
{
    [DataContract(Name="attribute"), Flags]
    public enum ParamAttribute
    {
        [EnumMember, XmlEnum("none")] 
        None = 0,
        [EnumMember, XmlEnum("in")]
        In = 0x1,
        [EnumMember, XmlEnum("out")]
        Out = 0x2,
        [EnumMember, XmlEnum("inout")]
        InOut = 0x4,
        [EnumMember, XmlEnum("buffer")]
        Buffer = 0x8,
        [EnumMember, XmlEnum("optional")]
        Optional = 0x10,
        /// <summary>
        /// Fast flag used for Out parameter.
        /// </summary>
        [EnumMember, XmlEnum("fast")]
        Fast = 0x20,
        [EnumMember, XmlEnum("params")]     // params setup buffer and params
        Params = 0x48,
        [EnumMember, XmlEnum("value")]     // force pointer to valuetype > 16 bytes to be passed by value
        Value = 0x80,
    }
}