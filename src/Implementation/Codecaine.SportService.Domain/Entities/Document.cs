using Codecaine.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Entities
{
    public class Document: AggregateRoot
    {
        public string Content { get; private set; }
        private readonly List<float> _embedding = [];
        public IReadOnlyCollection<float> Embedding => _embedding.AsReadOnly();

        public Document(string content, IEnumerable<float> embedding) : base(Guid.NewGuid())
        {
            Content = content;
            if (embedding != null)
            {
                _embedding.AddRange(embedding);
            }
        }

        public static Document Create(string content, IEnumerable<float> embedding)
        {
            return new Document(content, embedding);
        }
    }
}
