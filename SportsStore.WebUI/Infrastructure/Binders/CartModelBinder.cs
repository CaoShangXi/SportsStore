using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";
        /// <summary>
        /// 将Cart类添加到Session中，并返回Cart类
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //通过会话获取Cart
            Cart cart = null;
            if (controllerContext.HttpContext.Session!=null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            //若会话中没有Cart，则创建一个
            if (cart==null)
            {
                cart=new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            //返回cart
            return cart;
        }
    }
}