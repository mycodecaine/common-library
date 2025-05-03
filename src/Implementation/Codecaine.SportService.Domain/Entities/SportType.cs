using Codecaine.Common.Domain;
using Codecaine.Common.Domain.Interfaces;
using Codecaine.SportService.Domain.Events;

namespace Codecaine.SportService.Domain.Entities
{
    /// <summary>  
    /// Represents a type of sport, including its name, description, image, and associated variants.  
    /// </summary>  
    public class SportType : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>  
        /// Gets the name of the sport type.  
        /// </summary>  
        public string Name { get; private set; }

        /// <summary>  
        /// Gets the description of the sport type.  
        /// </summary>  
        public string Description { get; private set; }

        /// <summary>  
        /// Gets the URL of the image representing the sport type.  
        /// </summary>  
        public string ImageUrl { get; private set; }        

        /// <summary>  
        /// Initializes a new instance of the <see cref="SportType"/> class.  
        /// </summary>  
        public SportType() { }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SportType"/> class with the specified properties.  
        /// </summary>  
        /// <param name="name">The name of the sport type.</param>  
        /// <param name="description">The description of the sport type.</param>  
        /// <param name="imageUrl">The URL of the image representing the sport type.</param>  
        public SportType(string name, string description, string imageUrl) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
        }

        public static SportType Create(string name, string description, string imageUrl)
        {
            var  sportType = new SportType(name, description, imageUrl);
            sportType.AddDomainEvent(new SportTypeCreatedDomainEvent(sportType));

            return sportType;
        }

        public void Update(string name, string description, string imageUrl)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
        }

        // Audit Property  

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was created.  
        /// </summary>  
        public DateTime CreatedOnUtc { get; }

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was last modified.  
        /// </summary>  
        public DateTime? ModifiedOnUtc { get; }

        /// <summary>  
        /// Gets the identifier of the user who created the entity.  
        /// </summary>  
        public Guid? CreatedBy { get; }

        /// <summary>  
        /// Gets the identifier of the user who last modified the entity.  
        /// </summary>  
        public Guid? ModifiedBy { get; }

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was deleted.  
        /// </summary>  
        public DateTime? DeletedOnUtc { get; }

        /// <summary>  
        /// Gets a value indicating whether the entity has been soft deleted.  
        /// </summary>  
        public bool Deleted { get; }
    }
}
