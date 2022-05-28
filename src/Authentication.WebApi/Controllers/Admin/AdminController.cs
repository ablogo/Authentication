using Authentication.Core.Constants;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public class AdminController : ControllerBase
    {
        private readonly IAuthentication _authenticationService;
        private readonly IAccount _accountService;
        private readonly INotification _notificationService;
        private readonly IRole _roleService;

        public AdminController(IAuthentication authentication, IAccount account, INotification notification, IRole role)
        {
            _authenticationService = authentication;
            _accountService = account;
            _notificationService = notification;
            _roleService = role;
        }

        [HttpPost]
        public async Task<IActionResult> SeedDbCatalogs()
        {
            foreach (var item in Enum.GetNames<Roles>())
            {
                await _roleService.CreateRole(item);
            }

            return Ok();
        }

    }
}
