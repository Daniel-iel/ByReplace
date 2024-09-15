namespace ByReplace.Models
{
    internal class RuleBuilderTest
    {
        private string _name;
        private string _description;
        private string[] _skip;
        private string[] _extensions;
        private Replacement _replacement;

        public RuleBuilderTest()
        {

        }

        public static RuleBuilderTest Create()
        {
            return new RuleBuilderTest();
        }

        public RuleBuilderTest WithName(string name)
        {
            _name = name;
            return this;
        }

        public RuleBuilderTest WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public RuleBuilderTest WithSkip(params string[] skip)
        {
            _skip = skip;
            return this;
        }

        public RuleBuilderTest WithExtensions(params string[] extensions)
        {
            _extensions = extensions;
            return this;
        }

        public RuleBuilderTest WithReplacement(Action<Replacement> replacement)
        {
            _replacement = new Replacement([], "");
            replacement(_replacement);

            return this;
        }

        public Rule Build()
        {
            return new Rule(_name, _description, _skip, _extensions, _replacement);
        }
    }
}