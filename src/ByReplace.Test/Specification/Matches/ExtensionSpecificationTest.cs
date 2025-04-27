using ByReplace.Common;
using ByReplace.Specification.Matches;
using ByReplace.Test.TestHelpers.Builders;
using Xunit;

namespace ByReplace.Test.Specification.Matches
{
    public class ExtensionSpecificationTest
    {
        [Theory]
        [InlineData("C:\\dir\\appsettings.json", new string[] { ".json" })]
        [InlineData("C:\\dir\\file.cs", new string[] { ".json", ".cs" })]
        public void IsSatisfiedBy_WhenMatchExtension_ShouldReturnTrue(string fullName, string[] extensions)
        {
            // Arrange
            PathFixer path = new PathFixer();
            var pathParts = path.GetPathParts(fullName);

            var extensionSpecification = new ExtensionSpec(pathParts);

            var rule = RuleBuilderTest
                 .Create()
                 .WithName("Remove")
                 .WithDescription("ToRemove")
                 .WithSkip([])
                 .WithExtensions(extensions)
                 .WithReplacement(c =>
                 {
                     c = c with
                     {
                         Old = ["_test.", "this._test"],
                         New = "_test."
                     };
                 })
                 .Build();

            // Act
            var hasMatch = extensionSpecification.IsSatisfiedBy(rule);

            // Assert
            Assert.True(hasMatch);
        }

        [Theory]
        [InlineData("C:\\dir\\appsettings.json", new string[] { ".txt" })]
        [InlineData("C:\\dir\\file.cs", new string[] { ".json", ".md" })]
        public void IsSatisfiedBy_WhenDoesNotMatchExtension_ShouldReturnFalse(string fullName, string[] extensions)
        {
            // Arrange
            PathFixer path = new PathFixer();
            var pathParts = path.GetPathParts(fullName);

            var extensionSpecification = new ExtensionSpec(pathParts);

            var rule = RuleBuilderTest
                 .Create()
                 .WithName("Remove")
                 .WithDescription("ToRemove")
                 .WithSkip([])
                 .WithExtensions(extensions)
                 .WithReplacement(c =>
                 {
                     c = c with
                     {
                         Old = ["_test.", "this._test"],
                         New = "_test."
                     };
                 })
                 .Build();

            // Act
            var hasMatch = extensionSpecification.IsSatisfiedBy(rule);

            // Assert
            Assert.False(hasMatch);
        }
    }
}
