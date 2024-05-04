using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DbUserConversations.Common
{
    public class AuthenticationFunctions
    {
        public static bool IsAuthorizedToModifyUser(ClaimsPrincipal claimsPrincipal, string requestedId)
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (userId == requestedId)
            {
                return true;
            }

            return false;
        }
    }
}