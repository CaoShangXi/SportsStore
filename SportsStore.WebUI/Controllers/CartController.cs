using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    /// <summary>
    /// 购物车控制类，处理购物车相关的请求
    /// </summary>
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart">由于在Global和CarModelBinder有做配置，该参数的值会被框架自动注入</param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 添加物品到购物车
        /// </summary>
        /// <param name="cart">由于在Global和CarModelBinder有做配置，该参数的值会被框架自动注入</param>
        /// <param name="productId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        /// <summary>
        /// 从购物车删除物品
        /// </summary>
        /// <param name="cart">由于在Global和CarModelBinder有做配置，该参数的值会被框架自动注入</param>
        /// <param name="productId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        /// <summary>
        /// 返回模型给分部视图，分部视图即主视图的部分代码片段
        /// </summary>
        /// <param name="cart">由于在Global和CarModelBinder有做配置，该参数的值会被框架自动注入</param>
        /// <returns>返回模型给分部视图</returns>
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        /// <summary>
        /// 立即结算
        /// </summary>
        /// <returns>立即结算页面</returns>
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingInfo)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry,your cart is empty!");
            }
            //如果ShippingDetails类的送货信息验证通过
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingInfo);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingInfo);
            }
        }
    }
}