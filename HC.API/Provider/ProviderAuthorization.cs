using HC.BLL.GenericRepository.ConcRep;
using HC.UTILS.Common;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC.API.Provider
{
    //Startup clasında yazmış oldugumuz OAuthAuthorizationServerOptions ayarlarını tanımlarken Provider olarak OAuthAuthorizationServerProvider sınıfından miras alarak türeteceğimiz sağlayıcı sınıfı oluşturuyoruz.
    public class ProviderAuthorization : OAuthAuthorizationServerProvider
    {
        AppUserRepository appUserRepository = new AppUserRepository();

        // OAuthAuthorizationServerProvider sınıfının client erişimine izin verebilmek için ilgili ValidateClientAuthentication metotunu override ediyoruz.
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.CompletedTask;
        }

        // OAuthAuthorizationServerProvider sınıfının kaynak erişimine izin verebilmek için ilgili GrantResourceOwnerCredentials metotunu override ediyoruz.
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Domainler arası etkileşim ve kaynak paylaşımını sağlayan ve bir domainin bir başka domainin kaynağını kullanmasını sağlayan CORS ayarlarını set ediyoruz.
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //password kriptolama/şifreleme işlemi yapıyoruz.
            string passwordEncryption = Cryptography.Encryption(context.Password);

            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
            {
                context.SetError("Hata", "Kullanıcı Adı veya Şifre hatalı.");
                return Task.CompletedTask;
            }

            //Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
            var appUser = appUserRepository.FirstOrDefault(x => x.UserName == context.UserName && x.Password == context.Password && x.IsActive,new string[] { "AppUserRoles", "AppUserRoles.Role" });
            if (appUser != null)
            {
                List<string> appUserRoles = appUser.AppUserRoles.Select(x => x.Role.Name).ToList();

                ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("id", appUser.Id.ToString()));
                identity.AddClaim(new Claim("username", appUser.UserName));

                foreach (var roleName in appUserRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                }

                context.Validated(identity);
            }
            else
            {
                context.SetError("Hata", "Kullanıcı Adı veya Şifre hatalı.");
            }

            return Task.CompletedTask;
        }
    }
}