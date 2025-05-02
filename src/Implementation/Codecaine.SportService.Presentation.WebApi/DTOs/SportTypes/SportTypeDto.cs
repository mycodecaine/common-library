namespace Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes
{
    /// <summary>
    /// CreateSportTypeDto
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    public record SportTypeDto
    (
        string Name,
        string Description,
        string ImageUrl

    );
}
