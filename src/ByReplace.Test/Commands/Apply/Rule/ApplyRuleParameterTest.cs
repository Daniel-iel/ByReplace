using ByReplace.Commands.Apply.Parameters;
using ByReplace.Commands.Apply.Rule;
using ByReplace.Test.Common.Helpers;
using Cocona;
using Xunit;

namespace ByReplace.Test.Commands.Apply.Rule;

public class ApplyRuleParameterTest
{
    [Fact]
    public void ApplyRuleParameter_ShouldValidateTheApplyRuleParameterHasInheritancesFromApplyParameter()
    {
        // Arrange & Act
        var metadata = FileInspect<ApplyRuleParameter>.GetBaseType();

        // Assert
        Assert.NotNull(metadata);
        Assert.Equal(typeof(ApplyParameter), metadata);
    }

    [Fact]
    public void ApplyRuleParameter_ShouldValidateThePropertiesConfiguration()
    {
        // Arrange & Act
        var metadata = FileInspect<ApplyRuleParameter>.GetProperties();

        // Assert
        Assert.Equal(3, metadata.Length);
        Assert.Collection(metadata,
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(string), entry.Name.GetType());
            Assert.Equal("Rule", entry.Name);
            Assert.Single(attributes);
            var attribute = Assert.Single(attributes);
            var option = (OptionAttribute)attribute;

            Assert.Equal(typeof(OptionAttribute), attribute.GetType());
            Assert.Null(option.Name);
            Assert.Equal("Rule name to be applied.", option.Description);
            Assert.Collection(option.ShortNames,
            entry => Assert.Equal('r', entry));
        },
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(string), entry.Name.GetType());
            Assert.Equal("Path", entry.Name);
            Assert.Single(attributes);
            var attribute = Assert.Single(attributes);
            var option = (OptionAttribute)attribute;

            Assert.Equal(typeof(OptionAttribute), attribute.GetType());
            Assert.Null(option.Name);
            Assert.Equal("Path of the files to be applied to the rule.", option.Description);
            Assert.Collection(option.ShortNames,
            entry => Assert.Equal('p', entry));
        },
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(string), entry.Name.GetType());
            Assert.Equal("ConfigFile", entry.Name);
            Assert.Single(attributes);
            var attribute = Assert.Single(attributes);
            var option = (OptionAttribute)attribute;

            Assert.Equal(typeof(OptionAttribute), attribute.GetType());
            Assert.Null(option.Name);
            Assert.Equal("Path of the brconfig file.", option.Description);
            Assert.Collection(option.ShortNames,
            entry => Assert.Equal('f', entry));
        });
    }
}
