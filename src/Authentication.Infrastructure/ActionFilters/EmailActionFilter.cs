using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.ActionFilters
{
    public class EmailActionFilter : IAsyncActionFilter
    {

        private readonly IAccount _accountService;
        private readonly ILogger<EmailActionFilter> _log;

        public EmailActionFilter(IAccount account, ILogger<EmailActionFilter> log)
        {
            _accountService = account;
            _log = log;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (context.ActionArguments.TryGetValue("email", out var email))
                {
                    var account = await _accountService.GetUserByEmail(email.ToString());

                    if (account == null)
                    {
                        context.Result = new NotFoundResult();
                        return;
                    }
                    if (!account.IsActive)
                    {
                        context.Result = new NotFoundResult();
                        return;
                    }

                }
                else
                {
                    context.Result = new BadRequestResult();
                    return;
                }

            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException?.Message);
            }

            var result = await next();
        }


    }
}
