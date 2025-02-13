using SharpCastXml.CppModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Rendering {
    public interface IRenderable {
        void Init(CppElement model, IndentedTextWriter writer, RenderingContext renderingContext);
        void Render();
    }
}
