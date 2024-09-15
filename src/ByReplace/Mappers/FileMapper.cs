namespace ByReplace.Mappers;

[ExcludeFromCodeCoverage]
public record FileMapper(Guid id, string Name, string FullName, string Extension)
{
    public bool HasMatchToRule()
    {


        return false;
    }
};