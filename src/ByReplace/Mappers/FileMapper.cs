namespace ByReplace.Mappers;

[ExcludeFromCodeCoverage]
public record struct FileMapper(Guid id, string Name, string FullName, string Extension);