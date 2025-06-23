using Codecaine.Common.Domain;

namespace Codecaine.SportService.Domain.Entities
{

#pragma warning disable S125 // Sections of code should not be commented out
    /*
         * 
         * 
         * CREATE TABLE Document (

                    id UUID PRIMARY KEY,
                    name Text,
                    Description Text,
                    content TEXT,
                    embedding VECTOR(1536)  -- OpenAI embedding size
                );
         * 
         */
#pragma warning restore S125 // Sections of code should not be commented out
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
