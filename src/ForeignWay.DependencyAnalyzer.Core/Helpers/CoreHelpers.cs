using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ForeignWay.DependencyAnalyzer.Types;

namespace ForeignWay.DependencyAnalyzer.Helpers
{
    public  static class CoreHelpers
    {
        internal static IEnumerable<string> GetProjectFilePaths(string path, ICollection<string> excludePaths)
        {
            var projects = Directory.EnumerateFiles(path, "*.csproj", SearchOption.AllDirectories);

            if (excludePaths.Any() == false)
            {
                foreach (var project in projects)
                {
                    yield return project;
                }
                yield break;
            }
            
            foreach (var project in projects)
            {
                var found = false;
                foreach (var excludePath in excludePaths)
                {
                    if (project.StartsWith(excludePath)) found = true;
                }

                if (found == false)
                {
                    yield return project;
                }
            }
        }

        internal static IEnumerable<DependentAssembly> GetReferencesFromProject(string projectPath)
        {
            var text = File.ReadAllLines(projectPath);
            var referencedAssemblies = new List<DependentAssembly>();

            for (var i = 0; i < text?.Length; i++)
            { 
                if (text[i].Contains("<PackageReference") == false && text[i].Contains("<Reference") == false || text[i].Contains("<ReferencePath")) continue;
                
                var type = GetAssemblyNameFromXml(text[i]);
                var version = GetAssemblyVersionFromNewProjectStructureXml(text[i]);
                if (string.IsNullOrEmpty(version))
                    version = GetAssemblyVersionFromNewProjectStructureXml(text[i + 1]);

                referencedAssemblies.Add(new DependentAssembly(type, version, projectPath));
            }

            return referencedAssemblies;
        }

        internal static void ShowSeparator(string name)
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(name);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public  static ICollection<string> GetCollectionFromStringArg(string? argument)
        {
            if (string.IsNullOrEmpty(argument)) return new List<string>();

            var argCollection = argument.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x=> x.Trim());

            return argCollection.ToList();
        }

        private static string GetAssemblyNameFromXml(string xml)
        {
            var includeRegex = new Regex("\\\"([^\"^\\r\\n]*)\\\"");
            var includeMatches = includeRegex.Matches(xml);
            if (includeMatches.Count > 0)
                return includeMatches[0].ToString().Replace('"', ' ').Trim();

            throw new ArgumentException(nameof(xml));

            //return string.Empty;
        }

        private static string GetAssemblyVersionFromNewProjectStructureXml(string xml)
        {
            xml = xml.Trim();

            var versionRegex1 = new Regex(@"<Version>[\s\S]*?<\/Version>");
            var versionMatches = versionRegex1.Matches(xml);
            if (versionMatches.Count > 0)
            {
                var versionMatch = versionMatches[0].ToString();
                return versionMatch?.Replace("<Version>", string.Empty).Replace("</Version>", string.Empty).Trim() ??
                       string.Empty;
            }

            var versionRegex2 = new Regex(@"\""([^""^\r\n]*)\""");
            var versionMatches2 = versionRegex2.Matches(xml);

            return versionMatches2.Count >= 2 ? versionMatches2[1].ToString().Replace('"', ' ').Trim() : string.Empty;
        }

        private static string GetAssemblyVersionFromOldProjectStructureXml(string xml)
        {
            xml = xml.Trim();

            var versionRegex1 = new Regex(@"<Version>[\s\S]*?<\/Version>");
            var versionMatches = versionRegex1.Matches(xml);
            if (versionMatches.Count > 0)
            {
                var versionMatch = versionMatches[0].ToString();
                return versionMatch?.Replace("<Version>", string.Empty).Replace("</Version>", string.Empty).Trim() ?? string.Empty;
            }

            var versionRegex2 = new Regex(@"\""([^""^\r\n]*)\""");
            var versionMatches2 = versionRegex2.Matches(xml);

            return versionMatches2.Count >= 2 ? versionMatches2[1].ToString().Replace('"', ' ').Trim() : string.Empty;
        }
    }
}