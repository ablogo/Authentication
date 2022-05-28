using Authentication.Core.Constants;
using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ActionFilters;
using Authentication.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.WebApi.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authenticationService;
        private readonly IAccount _accountService;
        private readonly INotification _notificationService;
        private readonly IJwt _jwtService;

        public AuthenticationController(IAuthentication authentication, IAccount account, INotification notification, IJwt jwt)
        {
            _authenticationService = authentication;
            _accountService = account;
            _notificationService = notification;
            _jwtService = jwt;
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(LoginActionFilter))]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _authenticationService.Login(email, password, true);

            if (result.Succeeded)
            {
                var user = await _accountService.GetUserByEmail(email);
                var roles = await _accountService.GetUserRoles(user);

                result.Message = _jwtService.GenerateToken(user.Id, email, roles);

                return Ok(result.Message);
            }
            else
            {
                return Problem(result.Message, statusCode: StatusCodes.Status403Forbidden);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto new_user)
        {
            var user = new_user.ToEntity();

            var result = await _accountService.Create(user, new_user.Password);
            if (result.Succeeded && !string.IsNullOrEmpty(result.Message))
            {
                await _notificationService.SendEmailRequest(user.Id, new_user.Name, user.Email, result.Message, Notification.Types.EmailConfirmationAccount);
                return Ok(result.Message);
            }
            else 
            {
                return BadRequest(result.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> SendEmailResetPassword(string email)
        {
            var result = await _accountService.GetResetPasswordToken(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> ResetPassword(string email, string token, string new_password)
        {
            var result = await _accountService.ResetPassword(email, token, new_password);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> SendEmailAccountConfirmation(string email)
        {
            var result = await _accountService.GetEmailConfirmationToken(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> EmailConfirmation(string email, string token)
        {
            var result = await _accountService.EmailConfirmation(email, token);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> SendSMSAccountConfirmation(string email, string phone_number)
        {
            var result = await _accountService.GetSMSConfirmationCode(email, phone_number);
            if (result.Succeeded)
            {
                await _notificationService.SendSMSRequest(phone_number, "", result.Message, "", Notification.Types.SmsConfirmation);

                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ServiceFilter(typeof(EmailActionFilter))]
        public async Task<IActionResult> SMSConfirmation(string email, string phone_number, string code)
        {
            var result = await _accountService.PhoneConfirmation(email, phone_number, code);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }



    }
}
