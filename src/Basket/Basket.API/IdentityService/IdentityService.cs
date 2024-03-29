using System;
using Microsoft.AspNetCore.Http;

namespace Basket.API.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context; 

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.User.FindFirst("sub").Value;
        }
    }
}