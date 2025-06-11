using Codecaine.Common.Domain;

namespace Codecaine.SportService.Domain.Entities
{
    public class Document: AggregateVectorRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Document(string content, string name, string description) : base(Guid.NewGuid())
        {
            Content = content;           

            Name = name;
            Description = description;
        }

        public static Document Create( string name,string description,string content)
        {
            var document = new Document(content,name,description);
          
            return document;
        }
    }
}
