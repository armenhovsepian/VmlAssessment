namespace Vml.Core.Entities
{
    public class Link
    {
        public int Id { get; private set; }
        public string Rel { get; private set; }
        public string Href { get; private set; }
        public string Action { get; private set; }
        public string Types { get; private set; }

        public Guid DataJobId { get; private set; }
        public DataJob DataJob { get; private set; }

        private Link()
        {

        }

        public Link(string rel, string href, string action, string types)
        {
            Rel = rel;
            Href = href;
            Action = action;
            Types = types;
        }
    }
}
