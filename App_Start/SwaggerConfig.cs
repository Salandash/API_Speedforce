using System.Web.Http;
using WebActivatorEx;
using API_Speedforce;
using Swashbuckle.Application;
using System;
using System.Xml.XPath;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace API_Speedforce
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration 
                .EnableSwagger(c =>
                    {
                        c.IncludeXmlComments(GetXMLComments());
                    });
        }

        private static string GetXMLComments()
        {
            return System.String.Format(@"{0}\bin\API_Speedforce.XML", 
                System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
