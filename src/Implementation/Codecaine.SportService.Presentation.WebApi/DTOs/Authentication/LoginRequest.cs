namespace Codecaine.SportService.Presentation.WebApi.DTOs.Authentication
{
    public record LoginRequest
    (
        string Email,
        string Password
    );
}
