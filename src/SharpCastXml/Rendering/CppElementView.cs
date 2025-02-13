using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Figgle;
using SharpCastXml.CppModel;
using SharpCastXml.Rendering.CodeGen;

namespace SharpCastXml.Rendering
{
    public class ViewNameAttribute : Attribute
    {
        public string Name { get; }

        public ViewNameAttribute(string name)
        {
            Name = name;
        }
    }

    public abstract class CppElementView<TModel, TAnnotation> : IRenderable
        where TModel : CppElement { 

        private TAnnotation _annotation;
        private TModel _model;

        private IndentedTextWriter _writer;

        private RenderingContext _context;

        public TAnnotation Annotation => _annotation;

        public TModel Model => _model;

        public IndentedTextWriter Writer => _writer;

        public RenderingContext Context => _context;

        public void Init(CppElement model, IndentedTextWriter writer, RenderingContext context)
        {
            var m = model as TModel;
            if (m is null)
                throw new ArgumentException($"Model is not of type {typeof(TModel).Name}");
            Init(m, writer, context);
        }

        public virtual void Init(TModel model, IndentedTextWriter writer, RenderingContext context)
        {
            _model = model;
            _writer = writer;
            _context = context;

            if (Model.Annotation is null) {
                _annotation = Activator.CreateInstance<TAnnotation>();
            } else {
                _annotation = Model.Annotation.ToObject<TAnnotation>();
            }
        }

        public virtual void RenderView<TView>(CppElement model) 
            where TView : IRenderable
        {
            var view = Activator.CreateInstance<TView>() as IRenderable;

            typeof(TView).GetMethod("Init").Invoke(view, new []{ (object)model, Writer, Context});

            //view.Init(model, Writer, Container);
            view.Render();

        }

        public virtual void Render(CppElement model, IndentedTextWriter writer)
        {
            Context.Render(model, writer);
        }

        public virtual void Render(CppElement model)
        {
            Context.Render(model, Writer);
        }

        public abstract void Render();

        public virtual void W(string txt) => Writer.Write(txt);
        public virtual void WL(string txt = "") => Writer.WriteLine(txt);
        public virtual void Define(string name, object value = null) => WL($"#define {name} {value?.ToString() ?? ""}");
        public virtual void PushIndent() => Writer.Indent++;
        public virtual void PopIndent() => Writer.Indent++;

        public virtual void BannerFiglet(string name)
        {
            var figlet = FiggleFonts.Banner.Render(name).Split(new string[] {Environment.NewLine}, StringSplitOptions.None);

            foreach (var figLine in figlet)
            {
                WL("// " + figLine);
            }
        }

        public virtual void BannerFunction(string name)
        {
            WL("//" + new string('*', 70));
            WL($"//{name,70}");
            WL("//" + new string('*', 70));
        }

        public virtual IDisposable Function(string rtype, string name, string param) => new CodeRegion(
            () => { // Begin
                BannerFunction(name);
                WL($"{rtype} {name}({param}) {{");
                Writer.Indent++;
            },
            () => { // End
                Writer.Indent--;
                WL($"}} // {name}");
                WL();
                WL();
            });

        public virtual IDisposable NS(string[] ns) => new CodeRegion(
            () => { // Begin
                foreach (var n in ns)
                {
                    W($"namespace {n} {{ ");
                }
                Writer.Indent+=ns.Length;
            },
            () => { // End
                Writer.Indent-=ns.Length;
                WL($"{String.Join(" ", Enumerable.Repeat("}", ns.Length))} // {string.Join("::", ns)}");
                WL();
                WL();
            });

        public virtual IDisposable Indent() => new CodeRegion(
            () => Writer.Indent++,  // Begin
            () => Writer.Indent--); // End
        public virtual IDisposable IfDef(string macro) => new CodeRegion(
            () => WL("#ifdef " + macro),  // Begin
            () => WL("#endif // #ifdef " + macro)); // End
        public virtual IDisposable IfNDef(string macro) => new CodeRegion(
            () => WL("#ifndef " + macro),  // Begin
            () => WL("#endif // #ifndef " + macro)); // End

        public virtual void WalkStruct(CppStruct struct_, string outVar, int internalOffset, Action<CppFundamentalType, string, int> handler, bool writeInfo = true, bool writeContext = true) {
            foreach (var field in struct_.Fields) {
                if(writeInfo)
                    WL($"// {field.FullName}");
                WalkField(field, outVar, internalOffset, handler, writeInfo, writeContext);
            }
        }

        public virtual void WalkField(CppField field, string outVar, int internalOffset, Action<CppFundamentalType, string, int> handler, bool writeInfo = true, bool writeContext = true) {
            if (field.IsArray) {
                var arrSize = int.Parse(field.ArrayDimension);
                for (int i = 0; i < arrSize; i++) {
                    WalkDataType(field.Datatype, $"{outVar}.{field.Name}[{i}]", internalOffset + field.Offset / 8 + i * field.Datatype.Size / 8, handler, writeInfo, writeContext);
                }
            } else {
                WalkDataType(field.Datatype, $"{outVar}.{field.Name}", internalOffset + field.Offset / 8, handler, writeInfo, writeContext);
            }
        }

        public virtual void WalkDataType(CppDatatype dataType, string outVar, int internalOffset, Action<CppFundamentalType, string, int> handler, bool writeInfo = true, bool writeContext = true) {
            if (dataType is CppStruct @struct) {
                if (writeContext)
                    WL("{");
                using (Indent())
                    WalkStruct(@struct, outVar, internalOffset, handler, writeInfo, writeContext);
                if (writeContext)
                    WL("}");
            } else if (dataType is CppFundamentalType @type) {
                handler(@type, outVar, internalOffset);
            }
        }
    }
}
