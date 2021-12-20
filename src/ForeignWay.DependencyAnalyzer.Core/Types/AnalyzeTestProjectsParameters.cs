using System.Collections.Generic;

namespace ForeignWay.DependencyAnalyzer.Types
{
    public class AnalyzeTestProjectsParameters
    {
        public string SolutionDirectory { get; }
        public ICollection<string> ExcludedDirectories { get; }
        public ICollection<string> TestProjectNamePatterns { get; }
        public ICollection<string> TestProjectsNuGetPackages { get; }
        public bool Quiet { get; }


        public AnalyzeTestProjectsParameters(string solutionDirectory, ICollection<string>? excludedPaths,
            ICollection<string>? testProjectNamePatternsOption, ICollection<string>? testProjectsNuGetPackages, bool quiet)
        {
            SolutionDirectory = solutionDirectory;
            ExcludedDirectories = excludedPaths ?? new List<string>();
            TestProjectNamePatterns = testProjectNamePatternsOption ?? new List<string>();
            TestProjectsNuGetPackages = testProjectsNuGetPackages ?? new List<string>();
            Quiet = quiet;
        }
    }
}