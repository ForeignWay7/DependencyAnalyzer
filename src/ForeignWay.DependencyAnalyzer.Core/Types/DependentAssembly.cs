namespace ForeignWay.DependencyAnalyzer.Types
{ 
    public class DependentAssembly
    {
        public string Type { get;  }

        public string Version { get;  }

        public string SourceProj { get;  }


        public DependentAssembly(string type, string version, string sourceProj)
        {
            Type = type;
            Version = version;
            SourceProj = sourceProj;
        }

        public override string ToString()
        {
            return $"{Type}: {Version} ---> {SourceProj}";
        }
    }
}