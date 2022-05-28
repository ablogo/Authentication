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
    public class UserActionFilter : IAsyncActionFilter
    {

        private readonly IAccount _accountService;
        private readonly ILogger<UserActionFilter> _log;

        public UserActionFilter(IAccount account, ILogger<UserActionFilter> log)
        {
            _accountService = account;
            _log = log;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (context.ActionArguments.TryGetValue("user_id", out var user_id))
                {
                    var account = await _accountService.GetUser(user_id.ToString(), false);

                    if (account == null)
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
