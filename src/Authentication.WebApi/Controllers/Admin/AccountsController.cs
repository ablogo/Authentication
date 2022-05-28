using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ActionFilters;
using Authentication.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers.Admin
{/*
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountsController(IAccount account)
        {
            _account = account;
        }

        [HttpGet]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> GetUser(string user_id)
        {
            var result = await _account.GetUser(user_id);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> GetUserWithAddress(string user_id)
        {
            var result = await _account.GetUserWithAddress(user_id);
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> RegisterAddress(string user_id, AddressDto address)
        {
            await _account.CreateAddress(user_id, address);
            return Ok();
        }

        [HttpPost]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> ChangePassword(string user_id, string old_password, string new_password)
        {
            var result = await _account.UpdatePassword(user_id, old_password, new_password);
            return Ok(result);
        }

        [HttpPut]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> UpdateUser(string user_id, UserUpdate user)
        {
            var result = await _account.Update(user);
            return Ok(result);
        }

        [HttpPut]
        [ServiceFilter(typeof(UserActionFilter))]
        public async Task<IActionResult> UpdateAddress(string user_id, AddressDto address)
        {
            var result = await _account.UpdateAddress(user_id, address);
            return Ok(result);
        }
    }*/
}
