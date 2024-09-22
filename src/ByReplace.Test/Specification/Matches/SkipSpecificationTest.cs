using ByReplace.Models;
using ByReplace.Specification.Match;
using ByReplace.Test.TestHelpers.Builders;
using Xunit;

namespace ByReplace.Test.Specification.Matches;

public class SkipSpecificationTest
{
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

    [Theory]
    [InlineData("test.txt", "/dir/test.txt", "/dir", "test.txt")]
    [InlineData("test.txt", "/dir/test.txt", "/dir", "**\\dir\\*")]
    [InlineData("test.txt", "/dir/test.txt", "/dir", "/dir/test.txt")]
    public void SkipSpecification2(string fileName, string fullName, string dir, string skip)
    {
        // Arrange
        var fileMapper = FileMapperBuilderTest
           .Create()
           .WithName(fileName)
           .WithFullName(fullName)
           .WithExtension(".txt")
           .Build();

        var rule = RuleBuilderTest
            .Create()
            .WithName("Remove")
            .WithDescription("ToRemove")
            .WithSkip(skip)
            .WithExtensions(".txt")
            .WithReplacement(c =>
            {
                c = c with
                {
                    Old = ["_test.", "this._test"],
                    New = "_test."
                };
            })
            .Build();
        var spec = new SkipMatchSpecification(dir);

        // Act
        var result = spec.IsSatisfiedBy(fileMapper, rule);
        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("test.txt", "/dir/test.txt", "/dir", new string[] { "test.txt", "test1.txt" })]
    [InlineData("test.txt", "/dir/test.txt", "/folder", new string[] { "**\\dir\\*", "test1.txt" })]
    [InlineData("test.txt", "/dir/test.txt", "/folder", new string[] { "*\\dir\\*", "test1.txt" })]
    [InlineData("test.txt", "/dir/test.txt", "/folder", new string[] { "*\\dir\\", "test1.txt" })]
    [InlineData("test.txt", "/dir/test.txt", "/folder", new string[] { "\\dir\\", "test1.txt" })]
    [InlineData("test1.txt", "/dir/test1.txt", "/dir", new string[] { "/folder/test.txt", "test5.txt" })]
    public void SkipSpecification3(string fileName, string fullName, string dir, string[] skip)
    {
        // Arrange
        var fileMapper = FileMapperBuilderTest
           .Create()
           .WithName(fileName)
           .WithFullName(fullName)
           .WithExtension(".txt")
           .Build();

        var rule = RuleBuilderTest
            .Create()
            .WithName("Remove")
            .WithDescription("ToRemove")
            .WithSkip(skip)
            .WithExtensions(".txt")
            .WithReplacement(c =>
            {
                c = c with
                {
                    Old = ["_test.", "this._test"],
                    New = "_test."
                };
            })
            .Build();

        var spec = new SkipMatchSpecification(dir);

        // Act
        var result = spec.IsSatisfiedBy(fileMapper, rule);

        // Assert
        Assert.False(result);
    }

    //[Fact]
    //public void IsSatisfiedBy_ShouldReturnTrue_WhenSkipFileMatches()
    //{
    //    // Arrange
    //    var fileMapper = FileMapperBuilderTest
    //         .Create()
    //         .WithName("test.txt")
    //         .WithFullName("/dir/test.txt")
    //         .WithExtension(".txt")
    //         .Build();

    //    var rule = RuleBuilderTest
    //        .Create()
    //        .WithName("Remove")
    //        .WithDescription("ToRemove")
    //        .WithSkip("test.txt")
    //        .WithExtensions(".txt")
    //        .WithReplacement(c =>
    //        {
    //            c = c with
    //            {
    //                Old = ["_test.", "this._test"],
    //                New = "_test."
    //            };
    //        })
    //        .Build();

    //    //var file = new FileMapper { Name = "test.txt", FullName = "/dir/test.txt" }; // Substitua com a implementação correta de FileMapper
    //    //var rule = new Rule { Skip = new[] { "test.txt" } }; // Substitua com a implementação correta de Rule

    //    var spec = new SkipMatchSpecification("/dir");

    //    // Act
    //    var result = spec.IsSatisfiedBy(fileMapper, rule);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public void IsSatisfiedBy_ShouldReturnTrue_WhenSkipDirMatches()
    //{
    //    // Arrange
    //    var fileMapper = FileMapperBuilderTest
    //       .Create()
    //       .WithName("file.txt")
    //       .WithFullName("/dir/subdir/file.txt")
    //       .WithExtension(".txt")
    //       .Build();

    //    var rule = RuleBuilderTest
    //        .Create()
    //        .WithName("Remove")
    //        .WithDescription("ToRemove")
    //        .WithSkip("**subdir*")
    //        .WithExtensions(".txt")
    //        .WithReplacement(c =>
    //        {
    //            c = c with
    //            {
    //                Old = ["_test.", "this._test"],
    //                New = "_test."
    //            };
    //        })
    //        .Build();

    //    //var file = new FileMapper { Name = "file.txt", FullName = "/dir/subdir/file.txt" };
    //    //var rule = new Rule { Skip = new[] { "**subdir*" } };

    //    var spec = new SkipMatchSpecification("/dir/subdir");

    //    // Act
    //    var result = spec.IsSatisfiedBy(fileMapper, rule);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public void IsSatisfiedBy_ShouldReturnTrue_WhenSkipDirWithFileMatches()
    //{
    //    // Arrange
    //    var fileMapper = FileMapperBuilderTest
    //     .Create()
    //     .WithName("file.txt")
    //     .WithFullName("/dir/subdir/file.txt")
    //     .WithExtension(".txt")
    //     .Build();

    //    var rule = RuleBuilderTest
    //        .Create()
    //        .WithName("Remove")
    //        .WithDescription("ToRemove")
    //        .WithSkip("/subdir/file.txt")
    //        .WithExtensions(".txt")
    //        .WithReplacement(c =>
    //        {
    //            c = c with
    //            {
    //                Old = ["_test.", "this._test"],
    //                New = "_test."
    //            };
    //        })
    //        .Build();

    //    //var file = new FileMapper { Name = "file.txt", FullName = "/dir/subdir/file.txt" };
    //    //var rule = new Rule { Skip = new[] { "/subdir/file.txt" } };

    //    var spec = new SkipMatchSpecification("/dir/subdir");

    //    // Act
    //    var result = spec.IsSatisfiedBy(fileMapper, rule);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public void IsSatisfiedBy_ShouldReturnFalse_WhenNoSkipConditionsAreMet()
    //{
    //    // Arrange
    //    var fileMapper = FileMapperBuilderTest
    //     .Create()
    //     .WithName("file.txt")
    //     .WithFullName("/dir/subdir/file.txt")
    //     .WithExtension(".txt")
    //     .Build();

    //    var rule = RuleBuilderTest
    //        .Create()
    //        .WithName("Remove")
    //        .WithDescription("ToRemove")
    //        .WithSkip("**otherdir*")
    //        .WithExtensions(".txt")
    //        .WithReplacement(c =>
    //        {
    //            c = c with
    //            {
    //                Old = ["_test.", "this._test"],
    //                New = "_test."
    //            };
    //        })
    //        .Build();

    //    var spec = new SkipMatchSpecification("/dir/subdir");

    //    // Act
    //    var result = spec.IsSatisfiedBy(fileMapper, rule);

    //    // Assert
    //    Assert.False(result);
    //}
}
