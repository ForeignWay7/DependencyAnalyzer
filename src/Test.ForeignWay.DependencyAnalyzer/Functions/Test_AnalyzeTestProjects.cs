using ForeignWay.DependencyAnalyzer.Functions;
using ForeignWay.DependencyAnalyzer.Types;
using NUnit.Framework;

namespace Test.ForeignWay.DependencyAnalyzer.Functions
{
    [TestFixture]
    public class Test_AnalyzeTestProjects
    {
        [Test]
        public void Analyze() 
        { 
            var applicationArgs = new AnalyzeTestProjectsParameters(@"C:\Path\to\solution\directory", 
                null,
                new []
                {
                    "Test."
                }, new[]
                {
                    "Microsoft.NET.Test.Sdk", "NUnit3TestAdapter"
                }, false);

            var result = AnalyzeTestProjects.Analyze(applicationArgs);

            Assert.AreEqual(0, result);
        }
    }
}