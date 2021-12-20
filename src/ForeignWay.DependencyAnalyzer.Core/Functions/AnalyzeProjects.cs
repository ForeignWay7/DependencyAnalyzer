using System;
using System.Collections.Generic;
using System.Linq;
using ForeignWay.DependencyAnalyzer.Helpers;
using ForeignWay.DependencyAnalyzer.Types;

namespace ForeignWay.DependencyAnalyzer.Functions
{
    public static class AnalyzeProjects
    {
        public static int Analyze(AnalyzeProjectsParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SolutionDirectory)) throw new ArgumentNullException(nameof(parameters.SolutionDirectory));

            var projects = CoreHelpers.GetProjectFilePaths(parameters.SolutionDirectory, parameters.ExcludedDirectories);

            var assemblies = new List<DependentAssembly>();
            foreach (var project in projects)
            {
                assemblies.AddRange(CoreHelpers.GetReferencesFromProject(project));
            }

            if (parameters.Quiet == false)
            {
                Console.WriteLine();
                Console.WriteLine($"Found {projects.Count()} csproj files, {assemblies.Count} referenced assemblies");
            }

            var references = new List<string>();

            var duplicates = GetDuplicates(assemblies);

            if (parameters.Quiet == false)
                ShowDuplicates(duplicates, assemblies, references);

            return duplicates.Count > 0 ? -13 : 0;
        }

        public static IList<string> GetDuplicates(IEnumerable<DependentAssembly> assemblies)
        {
            var assemblyDictionary = new Dictionary<string, DependentAssembly>();
            
            var duplicates = new List<string>();
            foreach (var dependentAssembly in assemblies)
            {
                if (assemblyDictionary.ContainsKey(dependentAssembly.Type))
                {
                    if (assemblyDictionary[dependentAssembly.Type].Version.Equals(dependentAssembly.Version))
                        continue;

                    if (duplicates.Contains(dependentAssembly.Type) == false)
                        duplicates.Add(dependentAssembly.Type);
                }
                else
                {
                    assemblyDictionary.Add(dependentAssembly.Type, dependentAssembly);
                }
            }

            return duplicates;
        }

        private static void ShowDuplicates(IEnumerable<string> duplicates, IEnumerable<DependentAssembly> assemblies, ICollection<string> referencesList)
        {
            Console.ForegroundColor = duplicates.Any() ? ConsoleColor.Red : ConsoleColor.Green;

            CoreHelpers.ShowSeparator($"{duplicates.Count()} types referencing assemblies and/or NuGet packages in different versions found..");

            var orderedDuplicate = duplicates.OrderBy(x => x).ToArray();

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var duplicate in orderedDuplicate)
            {
                var references = assemblies
                    .Where(x => x.Type == duplicate);

                foreach (var reference in references)
                {
                    Console.WriteLine(reference);
                    referencesList.Add(reference.ToString());
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}