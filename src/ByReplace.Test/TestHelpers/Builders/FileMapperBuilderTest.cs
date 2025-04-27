using ByReplace.Mappers;

namespace ByReplace.Test.TestHelpers.Builders;

public class FileMapperBuilderTest
{
    private readonly Guid _id;
    private string _name;
    private string _fullName;
    private string _extension;

    public FileMapperBuilderTest()
    {
        _id = Guid.NewGuid();
    }

    public static FileMapperBuilderTest Create()
    {
        return new FileMapperBuilderTest();
    }

    public FileMapperBuilderTest WithName(string name)
    {
        _name = name;
        return this;
    }

    public FileMapperBuilderTest WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public FileMapperBuilderTest WithExtension(string extension)
    {
        _extension = extension;
        return this;
    }

    public FileMapper Build()
    {
        return new FileMapper(_id, _name, _fullName, _extension);
    }
}
