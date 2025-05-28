using Codecaine.Common.Domain.Interfaces;
using Codecaine.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codecaine.SportService.Domain.Events;

namespace Codecaine.SportService.Domain.Entities
{
    public class Player : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
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

        public override Guid Id => base.Id;

        public Player() { }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SportType"/> class with the specified properties.  
        /// </summary>  
        /// <param name="name">The name of the sport type.</param>  
        /// <param name="description">The description of the sport type.</param>  
        /// <param name="imageUrl">The URL of the image representing the sport type.</param>  
        public Player(string name, string description, string imageUrl) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
        }

        public static Player Create(string name, string description, string imageUrl)
        {
            var sportType = new Player(name, description, imageUrl);
           // Add Domain later sportType.AddDomainEvent(new SportTypeCreatedDomainEvent(sportType));

            return sportType;
        }

        // Audit Property  

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was created.  
        /// </summary>  
        public DateTime CreatedOnUtc { get; private set; }

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was last modified.  
        /// </summary>  
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>  
        /// Gets the identifier of the user who created the entity.  
        /// </summary>  
        public Guid? CreatedBy { get; private set; }

        /// <summary>  
        /// Gets the identifier of the user who last modified the entity.  
        /// </summary>  
        public Guid? ModifiedBy { get; private set; }

        /// <summary>  
        /// Gets the date and time in UTC format when the entity was deleted.  
        /// </summary>  
        public DateTime? DeletedOnUtc { get; private set; }

        /// <summary>  
        /// Gets a value indicating whether the entity has been soft deleted.  
        /// </summary>  
        public bool Deleted { get; private set; }
    }
}
