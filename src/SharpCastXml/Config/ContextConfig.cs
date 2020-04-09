using SharpCastXml.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCastXml.Config {
    public class ContextConfig {
        public List<IncludeDirRule> IncludeDirs { get; } = new List<IncludeDirRule>();
        public List<string> Include { get; set; } = new List<string>();
        public List<string> Process { get; set; } = new List<string>();
        public Dictionary<string, string> Macros { get; set; } = new Dictionary<string, string>();
        public string Id { get; set; } = "";
    }
}
