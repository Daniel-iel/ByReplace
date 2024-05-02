using ByReplace.Commands.Rule.ListRules;
using ByReplace.Test.Common.Helpers;
using Cocona;
using Xunit;

namespace ByReplace.Test.Commands.Rule.ListRules;

public class ListRulesParameterTest
{
    [Fact]
    public void ListRulesParameter_ShouldValidateTheListRulesParameterHasInheritancesFromICommandParameterSet()
    {
        // Arrange & Act
        var metadata = FileInspect<ListRulesParameter>.GetInterfaces();

        // Assert
        Assert.NotNull(metadata);
        Assert.Equal(typeof(ICommandParameterSet), metadata.First());
    }

    [Fact]
    public void ListRulesParameter_ShouldValidateThePropertiesConfiguration()
    {
        // Arrange & Act
        var metadata = FileInspect<ListRulesParameter>.GetProperties();

        // Assert
        Assert.Single(metadata);
        Assert.Collection(metadata,
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(string), entry.Name.GetType());
            Assert.Equal("ConfigFile", entry.Name);
            Assert.Single(attributes);
            Assert.Collection(attributes,
            attribute =>
            {
                var option = (OptionAttribute)attribute;

                Assert.Equal(typeof(OptionAttribute), attribute.GetType());
                Assert.Null(option.Name);
                Assert.Equal("Path of the brconfig file.", option.Description);
                Assert.Collection(option.ShortNames,
                entry => Assert.Equal('f', entry));
            });
        });
    }
}
