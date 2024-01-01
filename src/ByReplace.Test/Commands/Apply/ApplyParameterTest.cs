using ByReplace.Commands.Apply.Parameters;
using ByReplace.Test.Common.Helpers;
using Cocona;
using Xunit;

namespace ByReplace.Test.Commands.Apply;

public class ApplyParameterTest
{
    [Fact]
    public void ApplyParameter_ShouldValidateThePropertiesConfiguration1()
    {
        // Arrange & Act
        var metadata = FileInspect<ApplyParameter>.GetInterfaces();

        // Assert
        Assert.Equal(2, metadata.Length);
        Assert.Equal(typeof(ICommandParameterSet), metadata[0]);
        Assert.Equal(typeof(IEquatable<ApplyParameter>), metadata[1]);
    }

    [Fact]
    public void ApplyParameter_ShouldValidateThePropertiesConfiguration()
    {
        // Arrange & Act
        var metadata = FileInspect<ApplyParameter>.GetProperties();

        // Assert
        Assert.Equal(2, metadata.Length);
        Assert.Collection(metadata,
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(System.String), entry.Name.GetType());
            Assert.Equal("Path", entry.Name);
            Assert.Single(attributes);
            Assert.Collection(attributes,
            attribute =>
            {
                var option = (OptionAttribute)attribute;

                Assert.Equal(typeof(OptionAttribute), attribute.GetType());
                Assert.Null(option.Name);
                Assert.Equal("Path of the files to be applied to the rule.", option.Description);
                Assert.Collection(option.ShortNames,
                entry => Assert.Equal('p', entry));
            });
        },
        entry =>
        {
            var attributes = entry.GetCustomAttributes(false);

            Assert.Equal(typeof(System.String), entry.Name.GetType());
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
