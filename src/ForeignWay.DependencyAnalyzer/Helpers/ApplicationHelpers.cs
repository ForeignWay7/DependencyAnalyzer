using System;
using ForeignWay.DependencyAnalyzer.App.UserArguments;
using ForeignWay.DependencyAnalyzer.Helpers;
using ForeignWay.DependencyAnalyzer.Types;

namespace ForeignWay.DependencyAnalyzer.App.Helpers
{
    internal static class ApplicationHelpers
    {
        public static AnalyzeProjectsParameters MapUserArgsToAnalyzeProjectsParameters(UserArgs userArgs)
        {
            if (string.IsNullOrEmpty(userArgs.Command)) throw new ArgumentNullException($"argument {userArgs.Command} was null..");
            if (string.IsNullOrEmpty(userArgs.SolutionDirectory)) throw new ArgumentNullException($"argument {userArgs.SolutionDirectory} was null..");

            var excludedDirectories = CoreHelpers.GetCollectionFromStringArg(userArgs.DirectoriesFilterOption);
            var quiet = userArgs.QuietOption?.Equals("q") == true;

            return new AnalyzeProjectsParameters(userArgs.SolutionDirectory, excludedDirectories, quiet);
        }

        public static AnalyzeTestProjectsParameters MapUserArgsToAnalyzeTestProjectsParameters(UserArgs userArgs)
        {
            if (string.IsNullOrEmpty(userArgs.Command)) throw new ArgumentNullException($"argument {userArgs.Command} was null..");
            if (string.IsNullOrEmpty(userArgs.SolutionDirectory)) throw new ArgumentNullException($"argument {userArgs.SolutionDirectory} was null..");

            var testProjectNamePatterns = CoreHelpers.GetCollectionFromStringArg(userArgs.TestProjectsPatternOption);
            var excludedDirectories = CoreHelpers.GetCollectionFromStringArg(userArgs.DirectoriesFilterOption);
            var nugetPackages = CoreHelpers.GetCollectionFromStringArg(userArgs.TestProjectsNuGetPackages);
            var quiet = userArgs.QuietOption?.Equals("q") == true;

            return new AnalyzeTestProjectsParameters(userArgs.SolutionDirectory, excludedDirectories, testProjectNamePatterns, nugetPackages, quiet);
        }
    }
}