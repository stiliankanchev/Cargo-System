﻿namespace CargoSystem.Web
{
    using System.Reflection;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using CargoSystem.Web.Infrastructure.Mapping;
    using System;
    using System.Globalization;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
            var autoMapperConfig = new AutoMapperConfig(Assembly.GetExecutingAssembly());
            autoMapperConfig.Execute();

            ViewEnginesRegistration();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public class DateTimeBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                //var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                var date = DateTime.Parse(value.AttemptedValue, CultureInfo.InvariantCulture);

                return date;
            }
        }
        public class NullableDateTimeBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (value != null)
                {
                    var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                    return date;
                }
                return null;
            }
        }

        private static void ViewEnginesRegistration()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
