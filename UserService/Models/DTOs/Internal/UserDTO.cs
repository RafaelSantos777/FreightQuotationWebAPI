using Microsoft.Graph.Models;

namespace UserService.Models.DTOs.Internal;

public record UserDTO {

    public required string Id { get; init; }

    public required string DisplayName { get; init; }

    public required string Email { get; init; }

    public static UserDTO FromUser(User user) => new() { Id = user.Id!, DisplayName = user.DisplayName!, Email = user.Mail! };

}
