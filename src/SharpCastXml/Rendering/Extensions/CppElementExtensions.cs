using SharpCastXml.CppModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Rendering.Extensions
{
    public static class CppElementExtensions
    {
        public static string? GetViewName(this CppElement self)
        {
            if(self.Annotation is null)
            {
                return null;
            }

            if(self.Annotation.ContainsKey("view"))
            {
                return (string?)self.Annotation["view"];
            }

            return null;
        }
    }
}
