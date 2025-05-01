namespace Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes.CreateSportType
{
    /// <summary>
    /// CreateSportTypeDto
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    public record CreateSportTypeDto
    (
        string Name,
        string Description,
        string ImageUrl
       
    );
}
