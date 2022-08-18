using AutoMapper;
using HC.API.Mapping;
using System.Web.Http;

namespace HC.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(config => config.AddProfile(new MappingProfile()));

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
