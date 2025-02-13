using SharpCastXml.CppModel;
using SharpCastXml.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCastXml.TestProject
{
    public class TestAnnotation
    {
        public string hello { get; set; }
    }

    [ViewName("TestView")]
    public class TestView : CppElementView<CppStruct, TestAnnotation>
    {
        public override void Render()
        {
            WL(Annotation.hello);
        }
    }
}
