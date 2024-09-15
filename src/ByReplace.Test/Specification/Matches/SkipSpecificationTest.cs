using ByReplace.Models;
using ByReplace.Specification.Match;
using ByReplace.Test.TestHelpers.Builders;
using Xunit;

namespace ByReplace.Test.Specification.Matches
{
    public class SkipSpecificationTest
    {
        public SkipSpecificationTest()
        {

        }

        [Fact]
        public void SkipSpecification()
        {
            // Arrange
            var skipMatchSpecification = new SkipMatchSpecification("c:\\test\\");
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
            var hasMatch = skipMatchSpecification.IsSatisfiedBy(fileMapper, rule);

            // Assert
            Assert.True(hasMatch);
        }
    }
}
