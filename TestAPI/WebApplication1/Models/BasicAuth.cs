using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;
using System.Threading;
using System.Security.Principal;
using WebApplication1.Models;

public class BasicAuth : Attribute, IAuthorizationFilter
{
 
    public void OnAuthorization(AuthorizationFilterContext context)
    {
         var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
        {
             context.Result = new UnauthorizedResult();
            return;
        }

        var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

        if (StaticToken.Token(decodedUsernamePassword))
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(decodedUsernamePassword), null);
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
