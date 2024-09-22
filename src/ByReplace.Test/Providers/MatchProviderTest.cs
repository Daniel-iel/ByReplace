using ByReplace.Printers;
using ByReplace.Providers;
using ByReplace.Test.TestHelpers.ClassFixture;
using ByReplace.Test.TestHelpers.ConfigMock;
using ByReplace.Test.TestHelpers.FolderMock;
using Moq;
using Xunit;

namespace ByReplace.Test.Providers
{
    public class MatchProviderTest : IClassFixture<WorkspaceFixture<MatchProviderTest>>
    {
        private readonly WorkspaceFixture<MatchProviderTest> _fixture;
        private readonly Mock<IPrint> _printMock;

        public MatchProviderTest(WorkspaceFixture<MatchProviderTest> fixture)
        {
            _fixture = fixture;
            _printMock = new Mock<IPrint>();

            fixture.ClearPrevious();

            _fixture.WorkspaceSyntax = new WorkspaceSyntax(nameof(MatchProviderTest))
                .BRContent(c =>
                {
                    c.AddPath("")
                     .AddSkip("obj", ".bin")
                     .AddRules(
                        ruleOne => ruleOne
                                   .WithName("RuleTest")
                                   .WithExtensions(".cs", ".txt")
                                   .WithSkips("**\\Controllers\\*", "bin\\bin1.txt", "obj\\obj2.txt")
                                   .WithReplacement(BrContentFactory.Replacement("Test", "Test2")));
                })
                .Folder(folderStructure =>
                {
                    folderStructure
                        .AddFile(FileSyntax.FileDeclaration("RootFile1.cs", "ITest = new Test()"))
                        .AddFile(FileSyntax.FileDeclaration("RootFile2.cs", "ITest = new Test()"))
                        .AddFolder("Controllers", c =>
                        {
                            c.AddFile(FileSyntax.FileDeclaration("Controller1.cs", "ITest2 = new Test()"));
                            c.AddFile(FileSyntax.FileDeclaration("Controller2.cs", "ITest2 = new Test()"));
                        })
                        .AddFolder("bin", c =>
                        {
                            c.AddFile(FileSyntax.FileDeclaration("bin1.txt", "ITest = new Test()"));
                            c.AddFile(FileSyntax.FileDeclaration("bin2.txt", "ITest = new Test()"));
                        })
                        .AddFolder("obj", c =>
                        {
                            c.AddFile(FileSyntax.FileDeclaration("obj1.txt", "ITest = new Test()"));
                            c.AddFile(FileSyntax.FileDeclaration("obj2.txt", "ITest = new Test()"));
                        });
                })
                .Create();
        }

        [Fact]
        public void RunAnalysis_MapAllRulesThatMatchToFileInSourceTree_ShouldReturnRulesThatMatch()
        {
            // Arrange
            var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
            var analyzerRunner = new MatchProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object, analyzer);

            // Act
            var directoryNodes = analyzer.Run();
            var analyzersAndFixers = analyzerRunner.Run();

            // Assert
            var files = analyzersAndFixers.Keys.ToList();
            var rules = analyzersAndFixers.Values.ToList();

            Assert.Equal(4, analyzersAndFixers.Count);

            Assert.Collection(analyzersAndFixers,
            entry =>
            {
                Assert.Equal("RootFile1.cs", entry.Key.Name);
                Assert.Equal(".cs", entry.Key.Extension);
                Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
            },
            entry =>
            {
                Assert.Equal("RootFile2.cs", entry.Key.Name);
                Assert.Equal(".cs", entry.Key.Extension);
                Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
            },
            entry =>
            {
                Assert.Equal("bin2.txt", entry.Key.Name);
                Assert.Equal(".txt", entry.Key.Extension);
                Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
            },
            entry =>
            {
                Assert.Equal("obj1.txt", entry.Key.Name);
                Assert.Equal(".txt", entry.Key.Extension);
                Assert.Collection(entry.Value, rule => Assert.Equal("RuleTest", rule.Name));
            });
        }

        [Fact]
        public void RunAnalysis_WhenPrintLogInformation_ShouldValidateLogWasCalled()
        {
            // Arrange
            var analyzer = new SourceThreeProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object);
            var analyzerRunner = new MatchProvider(_fixture.WorkspaceSyntax.BrConfiguration, _printMock.Object, analyzer);

            // Act
            var directoryNodes = analyzer.Run();
            analyzerRunner.Run();

            // Assert
            _printMock.Verify(x => x.Information("Identifying rules that has matches."), Times.Once);
        }
    }
}