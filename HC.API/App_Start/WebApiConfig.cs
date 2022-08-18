using HC.API.Util;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HC.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //Api mizde xml desteğini kaldırmak istediğimizde ve text/html tipinde bir accept istek olduğunda json formatında response dönmesi için config ayarı ekleme yapılır
            //.
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;


            //Eğer dışarıda bulunan bir api projesine istekte bulunduğumuzda 'Acces-Control-Allow-Origin' hatası alırsak api projesinin oldugu yerdeki projeye nuget package manager'den Microsoft.AspNet.Cors kütüphanesi kurularak izinler verilir ve buraya webapiconfig'e cors tanımlanır.

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);  (* bütün sayfalara, bütün headerlara, bütün metotlara izin ver anlamında )

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);



            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
