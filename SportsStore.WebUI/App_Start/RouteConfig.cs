using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //此处路由配置优于后面的配置，所以要放在前面
            //routes.MapRoute(
            //    name:null,
            //    url:"Page{page}",//简化参数的格式，由原来的page=1改为page1
            //    defaults: new { controller = "Product", action = "List"}
            //    );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                null,
                "",
                new {controller="Product",action="List",category=(string)null,page=1}
                );

            routes.MapRoute(
                null,
                "Page{page}",
                new { controller = "Product", action = "List", category = (string)null, page = 1 },
                new {page=@"\d+"}
                );

            routes.MapRoute(
         null,
         "{category}",
         new { controller = "Product", action = "List", page = 1 }
         );

            routes.MapRoute(
                null,
                "{category}/Page{page}",
                new { controller = "Product", action = "List", category = (string)null, page = 1 },
                new { page = @"\d+" }
                );

            routes.MapRoute(null,"{controller}/{action}");
        }
    }
}
