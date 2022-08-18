using HC.API.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

//web api bearer token Authentication için gerekli owin kütüphaneleri yüklenir.
[assembly: OwinStartupAttribute(typeof(HC.API.App_Start.Startup))]
// Burada yukarıda tanımladığımız assembly ile serverımız ilk çalıştığında çalıştırılacak clasımızı tanımladık.Servis çalışmaya başlarken Owin pipeline'ını ayağa kaldırabilmek için Startup'u hazırlıyoruz.

namespace HC.API.App_Start
{
    public class Startup
    {

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);

            //Configuration metodunda gerekli http konfigürasyonları sağlanmaktadır. ConfigureOAuth metotunda token üretimi için gerekli authorization oluşturulmakta ve gerekli provider sınıfı tanımlanmaktadır.HttpConfiguration ayarlarımızı yaptıktan sonra App_Start’da yer alan Register metodumuzu bu configrasyon ile çağıracağız.
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            //Token üretimi için authorization ayarlarını belirliyoruz.
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/Authentication/Login"), //Token talebini yapacağımız dizin
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30), //Oluşturulacak tokenı bir gün geçerli olacak şekilde ayarlıyoruz.
                AllowInsecureHttp = true, //Güvensiz http portuna izin veriyoruz.
                Provider = new ProviderAuthorization() //Sağlayıcı sınıfını belirtiyoruz. Filter klasörü altında olusturuldu.
            };

            app.UseOAuthAuthorizationServer(OAuthOptions); //Yukarıda belirlemiş olduğumuz authorization ayarlarında çalışması için server'a ilgili OAuthAuthorizationServerOptions tipindeki nesneyi gönderiyoruz.

            //Authentication Type olarak Bearer Authentication'ı kullanacağımızı belirtiyoruz.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //Bearer Token, Herhangi bir kriptolu veriye ihtiyaç duyulmaksızın client tarafından token isteğinde bulunulur ve server belirli bir zamana sahip access_token üretir.
            
        }

    }
}
