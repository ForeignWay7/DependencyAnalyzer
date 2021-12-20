using CommandLine;

namespace ForeignWay.DependencyAnalyzer.App.UserArguments
{
    internal class UserArgs
    {
        [Option('c', "command", HelpText = "indicates the command to be processed.")]
        public string? Command { get; set; }
        

        [Option('d', "solution-directory", HelpText = "The solution directory to be analyzed for nuget references. This must be a directory.")]
        public string? SolutionDirectory { get; set; }

        
        [Option('e', "exclude-directories-filter", Default = null, HelpText = "Comma separated values text of directories to skip. Supports Starts with matching such as 'UnwantedDir*'")]
        public string? DirectoriesFilterOption { get; set; }

        
        [Option('t', "test-projects-pattern", Default = null, HelpText = "Comma separated values text of test projects to include. Supports Starts with matching such as 'Test.*'")]
        public string? TestProjectsPatternOption { get; set; }
        
        
        [Option('t', "test-projects-nuget-packages", Default = null, HelpText = "Comma separated values text of nuget packages that a test project must include.")]
        public string? TestProjectsNuGetPackages { get; set; }

        
        [Option('q', "quiet", Default = null, HelpText = "indicates weather a log should be displayed.")]
        public string? QuietOption { get; set; }
    }
}