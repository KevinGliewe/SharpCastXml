using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Rendering.CodeGen {
    public class CodeRegion : IDisposable {
        private readonly Action _regionEnd;

        public CodeRegion(Action begin, Action end) {
            _regionEnd = end;
            begin?.Invoke();
        }

        public void Dispose() {
            _regionEnd?.Invoke();
        }
    }
}
