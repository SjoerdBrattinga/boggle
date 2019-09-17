using Newtonsoft.Json.Serialization;
using System.Web.Http;
using static System.Web.Http.GlobalConfiguration;

namespace BoggleWebservice
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {            
            config.EnableCors();

            // Web API configuration and services
            Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            // Web API routes
            Configuration.MapHttpAttributeRoutes();

            Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
