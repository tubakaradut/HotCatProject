using HC.API.App_Start;
using HC.API.Models.DTOClasses;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;

namespace HC.API.Util
{
    //Swagger de manuel token oluşturabilmek için oluşturdugumuz method
    public class AuthUtil
    {
        public static string Token(AppUserDTO appUserDTO)
        {
            var identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
            identity.AddClaim(new Claim("id", appUserDTO.Id.ToString()));
            identity.AddClaim(new Claim("username", appUserDTO.UserName));

            foreach (var roleName in appUserDTO.RoleNames)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            }


            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(30));
            var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            return token;
        }

    }
}