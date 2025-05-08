using Codecaine.Common.Domain;
using Codecaine.Common.Domain.Interfaces;
using Codecaine.Common.Primitives.Ensure;
using Codecaine.SportService.Domain.Enumerations;
using Codecaine.SportService.Domain.Events;
using Codecaine.SportService.Domain.ValueObjects;

namespace Codecaine.SportService.Domain.Entities
{

    /// <summary>  
    /// Represents a variant of a sport, including its properties, rules, and associated player positions.  
    /// </summary>  
    public class SportVariant : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>  
        /// Gets the name of the sport variant.  
        /// </summary>  
        public string Name { get; private set; }

        /// <summary>  
        /// Gets the description of the sport variant.  
        /// </summary>  
        public string Description { get; private set; }

        /// <summary>  
        /// Gets the URL of the image representing the sport variant.  
        /// </summary>  
        public string ImageUrl { get; private set; }

        /// <summary>  
        /// Gets a value indicating whether the sport variant is part of the Olympic Games.  
        /// </summary>  
        public bool IsOlympic { get; private set; } = false;

        public Guid SportTypeId { get; private set; }
        /// <summary>  
        /// Gets the type of sport associated with this variant.  
        /// </summary>  
        public SportType SportType { get;  }

        /// <summary>  
        /// Gets the rules associated with this sport variant.  
        /// </summary>  
        public SportRule Rules { get; private set; }

        private readonly List<PopularInCountry> _popularInCountries = [];

        /// <summary>  
        /// Gets the list of countries where this sport variant is popular.  
        /// </summary>  
        public IReadOnlyCollection<PopularInCountry> PopularInCountries => _popularInCountries.AsReadOnly();

        private readonly List<PlayerPosition> _playerPositions = [];

        /// <summary>  
        /// Gets the list of player positions associated with this sport variant.  
        /// </summary>  
        public IReadOnlyCollection<PlayerPosition> PlayerPositions => _playerPositions.AsReadOnly();

        

        

        /// <summary>  
        /// Initializes a new instance of the <see cref="SportVariant"/> class.  
        /// </summary>  
        public SportVariant() { }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SportVariant"/> class with the specified properties.  
        /// </summary>  
        /// <param name="name">The name of the sport variant.</param>  
        /// <param name="description">The description of the sport variant.</param>  
        /// <param name="imageUrl">The URL of the image representing the sport variant.</param>  
        /// <param name="isOlympic">Indicates whether the sport variant is part of the Olympic Games.</param>  
        /// <param name="sportType">The type of sport associated with this variant.</param>  
        /// <param name="rules">The rules associated with this sport variant.</param>  
        public SportVariant(string name, string description, string imageUrl, bool isOlympic, Guid sportTypeId, SportRule rules) : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotEmpty(description, "The description is required.", nameof(description));

            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            IsOlympic = isOlympic;
            SportTypeId = sportTypeId;
            Rules = rules;
        }

        /// <summary>  
        /// Adds a country to the list of countries where this sport variant is popular.  
        /// </summary>  
        /// <param name="popularInCountry">The country code to add.</param>  
        public void AddPopularInCountry(CountryCode countryCode, int popularity)
        {
            if (_popularInCountries.Any(x=>x.CountryCode == countryCode))
                return;

            var popularInCountry = new PopularInCountry(countryCode, popularity);
            _popularInCountries.Add(popularInCountry);
        }

        public void UpdatePopularInCountry(Guid? id, CountryCode countryCode, int popularity)
        {
            if (_popularInCountries.Any(x => x.CountryCode != countryCode ) && id == null)
            {
                var newPopularInCountry = new PopularInCountry(countryCode, popularity);
                _popularInCountries.Add(newPopularInCountry);
                return;
            }

            var updatePopularInCountry = _popularInCountries.FirstOrDefault(x => x.Id == id);
            if (updatePopularInCountry is null)
                return;

            updatePopularInCountry.Update(countryCode, popularity);
        }

        /// <summary>  
        /// Removes a country from the list of countries where this sport variant is popular.  
        /// </summary>  
        /// <param name="countryCode">The country code to remove.</param>  
        public void RemovePopularInCountry(Guid id)
        {
            var country = _popularInCountries.FirstOrDefault(x => x.Id == id);
            if (country is null)
                return;
            _popularInCountries.Remove(country);
        }

        /// <summary>  
        /// Adds a new player position to the sport variant.  
        /// </summary>  
        /// <param name="name">The name of the player position.</param>  
        /// <param name="description">The description of the player position.</param>  
        /// <param name="imageUrl">The URL of the image representing the player position.</param>  
        /// <param name="responsibilities">The responsibilities of the player position.</param>  
        public void AddPlayerPosition(string name, string description, string imageUrl, string responsibilities)
        {
            if (_playerPositions.Any(x => x.Name.ToLower() == name.ToLower()))
                return;

            var playerPosition = new PlayerPosition(name, description, imageUrl, responsibilities);
            _playerPositions.Add(playerPosition);
        }

        public void UpdatePlayerPosition(Guid? id, string name, string description, string imageUrl, string responsibilities)
        {
            if (_playerPositions.Any(x => x.Name.ToLower() != name.ToLower()) && id == null)
            {
                var newPlayerPosition = new PlayerPosition(name, description, imageUrl, responsibilities);
                _playerPositions.Add(newPlayerPosition);
                return;
            }
            var updatePlayerPosition = _playerPositions.FirstOrDefault(x => x.Id == id);
            if (updatePlayerPosition is null)
                return;
            updatePlayerPosition.Update(name, description, imageUrl, responsibilities);
        }

        /// <summary>  
        /// Removes a player position from the sport variant by its identifier.  
        /// </summary>  
        /// <param name="playerPositionId">The identifier of the player position to remove.</param>  
        public void RemovePlayerPosition(Guid playerPositionId)
        {
            var playerPosition = _playerPositions.FirstOrDefault(x => x.Id == playerPositionId);
            if (playerPosition is null)
                return;
            _playerPositions.Remove(playerPosition);
        }

        /// <summary>  
        /// Updates an existing player position in the sport variant.  
        /// </summary>  
        /// <param name="playerPositionId">The identifier of the player position to update.</param>  
        /// <param name="name">The new name of the player position.</param>  
        /// <param name="description">The new description of the player position.</param>  
        /// <param name="imageUrl">The new URL of the image representing the player position.</param>  
        /// <param name="responsibilities">The new responsibilities of the player position.</param>  
        public void UpdatePlayerPosition(Guid playerPositionId, string name, string description, string imageUrl, string responsibilities)
        {
            var playerPosition = _playerPositions.FirstOrDefault(x => x.Id == playerPositionId);
            if (playerPosition is null)
                return;
            playerPosition.Update(name, description, imageUrl, responsibilities);
        }

        /// <summary>  
        /// Creates a new instance of the <see cref="SportVariant"/> class and raises a domain event.  
        /// </summary>  
        /// <param name="name">The name of the sport variant.</param>  
        /// <param name="description">The description of the sport variant.</param>  
        /// <param name="imageUrl">The URL of the image representing the sport variant.</param>  
        /// <param name="isOlympic">Indicates whether the sport variant is part of the Olympic Games.</param>  
        /// <param name="sportType">The type of sport associated with this variant.</param>  
        /// <param name="rules">The rules associated with this sport variant.</param>  
        /// <returns>A new instance of the <see cref="SportVariant"/> class.</returns>  
        public static SportVariant Create(string name, string description, string imageUrl, bool isOlympic, Guid sportTypeId, SportRule rules)
        {
            var sportVariant = new SportVariant(name, description, imageUrl, isOlympic, sportTypeId, rules);

            sportVariant.AddDomainEvent(new SportVariantCreatedDomainEvent(sportVariant));

            return sportVariant;
        }

        /// <summary>  
        /// Updates the properties of the sport variant and raises a domain event.  
        /// </summary>  
        /// <param name="name">The new name of the sport variant.</param>  
        /// <param name="description">The new description of the sport variant.</param>  
        /// <param name="imageUrl">The new URL of the image representing the sport variant.</param>  
        /// <param name="isOlympic">Indicates whether the sport variant is part of the Olympic Games.</param>  
        /// <param name="sportType">The new type of sport associated with this variant.</param>  
        /// <param name="rules">The new rules associated with this sport variant.</param>  
        public void Update(string name, string description, string imageUrl, bool isOlympic, Guid sportType, SportRule rules)
        {
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotEmpty(description, "The description is required.", nameof(description));

            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            IsOlympic = isOlympic;
            SportTypeId = sportType;
            Rules = rules;
            AddDomainEvent(new SportVariantUpdatedDomainEvent(this));
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
