using UserService.Models.DTOs.Internal;

namespace UserService.Interfaces;

public interface IUserQueryService {

    Task<UserDTO?> GetUser(string id);

    Task<IEnumerable<UserDTO>> SearchUsers(string? search);

    Task<(IEnumerable<UserDTO> deletedUserDTOs, string nextDeltaLink)> GetDeletedUsers(string? deltaLink);

}
