using Codecaine.Common.Domain;

namespace Codecaine.SportService.Domain.Entities
{
    public class PlayerPosition : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }        
        public string Responsibilities { get; private set; }

        // Parameterless constructor for EF Core
        public PlayerPosition()
        {
        }

        public PlayerPosition(string name, string description, string imageUrl, string responsibilities) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Responsibilities = responsibilities;
        }

        public void Update(string name, string description, string imageUrl, string responsibilities)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Responsibilities = responsibilities;
        }
    }
}
