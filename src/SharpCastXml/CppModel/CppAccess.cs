using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.CppModel
{
    public enum CppAccess
    {
        Public,
        Protected,
        Private,
    }

    public static class CppAccessExtensions
    {
        public static CppAccess FromString(string access)
        {
            switch (access)
            {
                case "public":
                    return CppAccess.Public;
                case "protected":
                    return CppAccess.Protected;
                case "private":
                    return CppAccess.Private;
                default:
                    throw new ArgumentException($"Unknown access specifier: {access}");
            }
        }
    }

}