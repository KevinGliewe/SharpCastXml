using SharpCastXml.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Config {
    public class ContextConfig {
        public List<IncludeDirRule> IncludeDirs { get; } = new List<IncludeDirRule>();
        public List<string> Includes { get; set; } = new List<string>();
        public string Id { get; set; } = "";
    }
}
