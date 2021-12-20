using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ForeignWay.DependencyAnalyzer.Helpers;
using ForeignWay.DependencyAnalyzer.Types;

namespace ForeignWay.DependencyAnalyzer.Functions
{
    public static class AnalyzeTestProjects
    {
        public static int Analyze(AnalyzeTestProjectsParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SolutionDirectory)) throw new ArgumentNullException(nameof(parameters.SolutionDirectory));
            if (parameters.TestProjectNamePatterns.Any() == false) throw new ArgumentNullException(nameof(parameters.TestProjectNamePatterns));
            if (parameters.TestProjectsNuGetPackages.Any() == false) throw new ArgumentNullException(nameof(parameters.TestProjectsNuGetPackages));

            var projects = GetTestProjectFilePaths(parameters.SolutionDirectory, parameters.TestProjectNamePatterns, parameters.ExcludedDirectories);

            var failingProjects = new List<string>();

            foreach (var project in projects)
            {
                var assemblies = CoreHelpers.GetReferencesFromProject(project);
                var arePackagesReferenced = CheckIfPackagesAreReferenced(assemblies, parameters.TestProjectsNuGetPackages);

                if (arePackagesReferenced == false)
                    failingProjects.Add(project);
            }

            if (failingProjects.Any())
            {
                if (parameters.Quiet) return -17;
                
                Console.WriteLine($"Found {failingProjects.Count} csproj files that are not referencing Test packages:");

                foreach (var projectFileName in failingProjects.Select(failingProject => new FileInfo(failingProject).Name))
                {
                    Console.WriteLine(projectFileName);
                }

                return -17;
            }

            return 0;
        }

        private static IEnumerable<string> GetTestProjectFilePaths(string path, IEnumerable<string> projectNamePatterns, ICollection<string> excludedPaths)
        {
            var filteredProjects = new List<string>();

            foreach (var projectNamePattern in projectNamePatterns)
            {
                var projects = Directory.EnumerateFiles(path, "*.csproj", SearchOption.AllDirectories)
                    .Where(x => new FileInfo(x).Name.StartsWith(projectNamePattern));

                foreach (var project in projects)
                {
                    if (excludedPaths?.Any() == true)
                    {
                        filteredProjects.AddRange(from excludedPath in excludedPaths where project.StartsWith(excludedPath) == false select project);
                        continue;
                    } 
                    
                    filteredProjects.Add(project);
                }
            }

            return filteredProjects;
        }

        private static bool CheckIfPackagesAreReferenced(IEnumerable<DependentAssembly> assemblies, ICollection<string> nugetPackages)
        {
            var arePackageReferenced = assemblies.Select(x => x.Type).Intersect(nugetPackages).Count() == nugetPackages.Count;

            return arePackageReferenced;
        }
    }
}