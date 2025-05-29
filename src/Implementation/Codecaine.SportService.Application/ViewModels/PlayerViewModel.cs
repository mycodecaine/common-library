namespace Codecaine.SportService.Application.ViewModels
{

    public record PlayerViewModel

        (
        Guid Id,
        string Name,
        string Description,
        string ImageUrl
        );
}
