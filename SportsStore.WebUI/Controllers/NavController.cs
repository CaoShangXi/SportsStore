using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    /// <summary>
    /// 用于演示ASP.NET MVC框架的子动作概念，即同一个视图可以执行多个Controller的动作方法
    /// </summary>
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category=null,bool horizontalLayout=false)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories=repository.Products.Select(x=>x.Category).Distinct().OrderBy(x=>x);
            return PartialView("FlexMenu",categories);//将model传递给视图，并返回分部视图给主视图
        }
    }
}