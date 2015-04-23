using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using TestDelivery.ServiceLayer;
using TestDelivery.UI.Helpers;

namespace TestDelivery.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitDependencyInjection();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void InitDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                   .PropertiesAutowired();

            RegisterServices(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private const string SERVICE_CONVENTION_SUFFIX = "Service";
        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(TestService).Assembly)
                   .Where(t => t.Name.EndsWith(SERVICE_CONVENTION_SUFFIX))
                   .PropertiesAutowired(PropertyWiringFlags.Default)
                   .AsImplementedInterfaces();

            builder.RegisterType<QuestionViewService>()
                   .AsImplementedInterfaces();
        }
    }
}