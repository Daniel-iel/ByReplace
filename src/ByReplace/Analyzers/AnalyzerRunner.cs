
namespace ByReplace.Analyzers
{
    internal class AnalyzerRunner
    {
        private readonly BrConfiguration brConfiguration;
        private readonly ImmutableList<DirectoryNode> directoryThree;

        internal AnalyzerRunner(
            BrConfiguration brConfiguration,
            ImmutableList<DirectoryNode> filesThree)
        {
            this.brConfiguration = brConfiguration;
            this.directoryThree = filesThree;
        }

        internal AnalyzersAndFixers RunAnalysis(Analyses diagnostic)
        {
            AnalyzersAndFixers analyzersAndFixers = new AnalyzersAndFixers();

            foreach (DirectoryNode dir in directoryThree)
            {
                analyzersAndFixers.TryMatchRole(dir, brConfiguration.Rules);
            }

            return analyzersAndFixers;
        }
    }
}
