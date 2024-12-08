using ByReplace.Specification.Matches;
using ByReplace.Test.TestHelpers.Builders;
using Xunit;

namespace ByReplace.Test.Specification.Matches
{
    public class ExtensionSpecificationTest
    {
        [Fact]
        public void IsSatisfiedBy_IdentifyFileThatMatchByExtension_ShouldReturnTrue()
        {
            // Arrange
            var extensionSpecification = new SkipExtensionSpec([]);
            var fileMapper = FileMapperBuilderTest
                 .Create()
                 .WithName("test")
                 .WithFullName("c:\\test\\test.cs")
                 .WithExtension(".cs")
                 .Build();

            var rule = RuleBuilderTest
                 .Create()
                 .WithName("Remove")
                 .WithDescription("ToRemove")
                 .WithSkip(".json")
                 .WithExtensions(".cs")
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
    }
}
