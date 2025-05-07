namespace Codecaine.SportService.Application.DTOs
{
    public record PlayerPositionDto
    (
        string Name,
        string Description,
        string ImageUrl,
        string Responsibilities,
        Guid? Id = null
    );
}
