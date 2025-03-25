using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JoyCase.Api.Infrastructure
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public PermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasPermission = user.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .Contains(_permission);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
