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
    public class AccountActionFilter : IAsyncActionFilter
    {

        private readonly IAccount _accountService;
        private readonly ILogger<AccountActionFilter> _log;

        public AccountActionFilter(IAccount account, ILogger<AccountActionFilter> log)
        {
            _accountService = account;
            _log = log;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var user_id = context.HttpContext.User.Identity.Name;

                if (!string.IsNullOrEmpty(user_id))
                {
                    var account = await _accountService.GetUser(user_id, false);

                    if (account != null)
                    {
                        context.HttpContext.Items.Add("account", account);
                    }
                    else
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
