using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using ForeignWay.DependencyAnalyzer.App.Helpers;
using ForeignWay.DependencyAnalyzer.App.UserArguments;
using ForeignWay.DependencyAnalyzer.Functions;

namespace ForeignWay.DependencyAnalyzer.App
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<UserArgs>(args);
            
            return await result.MapResult(Execute, errors => Task.FromResult(1));
        }

        private static async Task<int> Execute(UserArgs args)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(args.Command))
                {
                    ShowMessage(-3);
                    return await Task.FromResult(1);
                }
                if (string.IsNullOrWhiteSpace(args.SolutionDirectory))
                {
                    ShowMessage(-9);
                    return await Task.FromResult(-9);
                }
                if (Directory.Exists(args.SolutionDirectory) == false)
                {
                    ShowMessage(-10);
                    return await Task.FromResult(-10);
                }

                int result;

                switch (args.Command)
                {
                    case nameof(SupportedCommands.AnalyzeProjects):
                        var projParam = ApplicationHelpers.MapUserArgsToAnalyzeProjectsParameters(args);
                        result = AnalyzeProjects.Analyze(projParam);
                        break;

                    case nameof(SupportedCommands.AnalyzeTestProjects):
                        var testProjParam = ApplicationHelpers.MapUserArgsToAnalyzeTestProjectsParameters(args);
                        result = AnalyzeTestProjects.Analyze(testProjParam);
                        break;

                    default:
                        ShowMessage(-4);
                        return await Task.FromResult(-4);
                }

                ShowMessage(result);
                return await Task.FromResult(result);
            }
            catch
            {
                ShowMessage(-1);
                return await Task.FromResult(-1);
            }
        }

        private static void ShowMessage(int exitCode)
        {
            var resultMessage = exitCode switch
            {
                0 => "Res(0):\tAnalysis successful and did not log any error.",
                -3 => "ERR(-3):\tA command must be specified!",
                -4 => "ERR(-4):\tThe given command is not recognized!",
                -9 => "ERR(-9):\tA solution path was not specified!",
                -10 => "ERR(-10):\tThe solution directory does not exist!",
                -13 => "Res(-13):\tProjects are referencing assemblies and/or NuGet packages in different versions!",
                -17 => "Res(-17):\tTest projects are not referencing Test NuGet packages!",
                _ => $"ERR({exitCode}):\tAn unknown error occurred.."
            };

            Console.WriteLine();
            Console.WriteLine(resultMessage);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
