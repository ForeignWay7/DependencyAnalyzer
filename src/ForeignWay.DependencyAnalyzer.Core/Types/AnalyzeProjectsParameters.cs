using System.Collections.Generic;

namespace ForeignWay.DependencyAnalyzer.Types
{
    public class AnalyzeProjectsParameters
    {
        public string SolutionDirectory { get; }
        public ICollection<string> ExcludedDirectories { get; }
        public bool Quiet { get; }


        public AnalyzeProjectsParameters(string solutionDirectory, ICollection<string>? excludedPaths, bool quiet)
        {
            SolutionDirectory = solutionDirectory;
            ExcludedDirectories = excludedPaths ?? new List<string>();
            Quiet = quiet;
        }
    }
}