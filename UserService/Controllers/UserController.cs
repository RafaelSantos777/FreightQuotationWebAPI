using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Shared;
using UserService.Interfaces;
using UserService.Models.DTOs.Internal;

namespace UserService.Controllers;

[Route("[controller]s")]
public class UserController(IUserQueryService userQueryService) : ControllerBase {

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetSelf() {
        UserDTO? userDTO = await userQueryService.GetUser(User.GetObjectId()!);
        if (userDTO is null)
            throw new Exception("Authenticated user not found");
        return Ok(userDTO);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Administrator)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById([FromRoute] string id) {
        UserDTO? userDTO = await userQueryService.GetUser(id);
        return userDTO is null ? NotFound() : Ok(userDTO);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Search([FromQuery] string? search) {
        IEnumerable<UserDTO> userDTOs = await userQueryService.SearchUsers(search);
        return Ok(userDTOs);
    }

}
