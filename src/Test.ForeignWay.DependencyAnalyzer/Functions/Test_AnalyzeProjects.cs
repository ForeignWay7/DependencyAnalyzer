using ForeignWay.DependencyAnalyzer.Functions;
using ForeignWay.DependencyAnalyzer.Types;
using NUnit.Framework;

namespace Test.ForeignWay.DependencyAnalyzer.Functions
{
    [TestFixture]
    public class Test_AnalyzeProjects
    {
        [Test]
        public void Analyze()
        {
            var applicationArgs = new AnalyzeProjectsParameters(@"C:\Path\to\solution\directory", null, false);
            
           var result = AnalyzeProjects.Analyze(applicationArgs);

           Assert.AreEqual(0, result);
        }

        [Test]
        public void Analyze_WithFilteredDirectory()
        {
            var applicationArgs = new AnalyzeProjectsParameters(@"C:\Path\to\solution\directory", new[]
                {
                    @"C:\Path\to\solution\directory\directoryToBeExcluded"
                }, false);
            
           var result = AnalyzeProjects.Analyze(applicationArgs);

           Assert.AreEqual(0, result);
        }
    }
}