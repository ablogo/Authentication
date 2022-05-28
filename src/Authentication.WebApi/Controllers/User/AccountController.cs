using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ActionFilters;
using Authentication.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers.User
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }

        [HttpGet]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> GetUser(bool include_address = false)
        {
            var user_id = HttpContext.User.Identity.Name;

            var result = await _account.GetUser(user_id, include_address);
            return Ok(result.ToDto());
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> RegisterAddress(AddressDto address)
        {
            var user_id = HttpContext.User.Identity.Name;

            await _account.CreateAddress(user_id, address);
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> DeleteUser()
        {
            var user_id = HttpContext.User.Identity.Name;

            await _account.Delete(user_id);
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> ChangePassword(string old_password, string new_password)
        {
            var user_id = HttpContext.User.Identity.Name;

            var result = await _account.UpdatePassword(user_id, old_password, new_password);
            return Ok(result);
        }

        [HttpPut]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> UpdateUser(UserUpdate user_dto)
        {
            //Todo
            user_dto.Id = HttpContext.User.Identity.Name;

            var result = await _account.Update(user_dto);
            return Ok();
        }

        [HttpPut]
        [ServiceFilter(typeof(AccountActionFilter))]
        public async Task<IActionResult> UpdateAddress(AddressDto address)
        {
            //Todo
            var user_id = HttpContext.User.Identity.Name;

            var result = await _account.UpdateAddress(user_id, address);
            return Ok();
        }
    }
}
