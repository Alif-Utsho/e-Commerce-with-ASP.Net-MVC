using ECMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace ECMS.App_Start
{
    public class UnityConfig
    {
        public static IUnityContainer Container { get; internal set; }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register your service
            container.RegisterSingleton<GlobalDataService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}