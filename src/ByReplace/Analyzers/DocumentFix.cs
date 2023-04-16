namespace ByReplace.Analyzers
{
    internal class DocumentFix
    {
        private readonly AnalyzersAndFixers codeFixes;

        public DocumentFix(AnalyzersAndFixers codeFixes)
        {
            this.codeFixes = codeFixes;
        }

        public void Apply()
        {
            foreach (var codeFixe in this.codeFixes)
            {
                Rule role = codeFixe.Key;
                foreach (var file in codeFixe.Value)
                {
                    foreach (var removeTerm in role.Replacement.Old)
                    {
                        string fileContents = File.ReadAllText(file.FullName);
                        fileContents = fileContents.Replace(removeTerm, role.Replacement.New);
                        File.WriteAllText(file.FullName, fileContents);
                    }
                }
            }
        }
    }
}
