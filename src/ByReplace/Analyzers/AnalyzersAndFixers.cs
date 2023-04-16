namespace ByReplace.Analyzers
{
    internal class AnalyzersAndFixers : Dictionary<Rule, List<FileMapper>>
    {
        public bool TryMatchRole(DirectoryNode directoryNode, ImmutableList<Rule> roles)
        {
            foreach (var file in directoryNode.Files)
            {
                foreach (var role in roles)
                {
                    Match skipDirMatch = new SkipMatch(directoryNode.Directory, file, role.Skip);
                    Match extensionMatch = new ExtensionMatch(file.Extension, role.Extensions);

                    if (skipDirMatch.HasMatch || !extensionMatch.HasMatch)
                    {
                        continue;
                    }

                    if (!this.ContainsKey(role))
                    {
                        this.Add(role, new List<FileMapper>());
                    }

                    this[role].Add(file);
                }
            }

            return false;
        }
    }
}
