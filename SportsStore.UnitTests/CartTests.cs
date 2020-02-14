using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// 测试购物车的功能
    /// </summary>
    [TestClass]
    public class CartTests
    {
        /// <summary>
        /// 测试添加物品到购物车
        /// </summary>
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //准备-创建一个新的购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();
            //断言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        /// <summary>
        /// 测试用户重复添加物品时，物品数量是否已更新
        /// </summary>
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_lines()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //准备-创建一个新的购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(x => x.Product.ProductID).ToArray();
            //断言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        /// <summary>
        /// 测试从购物车删除物品
        /// </summary>
        [TestMethod]
        public void Can_Remove_Lines()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //准备-创建一个新的购物车
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            target.RemoveLine(p2);
            //断言
            Assert.AreEqual(target.Lines.Where(x => x.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        /// <summary>
        /// 测试计算购物车里面物品的总金额
        /// </summary>
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //准备-创建一个新的购物
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            //断言
            Assert.AreEqual(result, 450M);
        }

        /// <summary>
        /// 测试清空购物车的所有物品
        /// </summary>
        [TestMethod]
        public void Can_Clear_Contents()
        {
            //准备-创建一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //准备-创建一个新的购物
            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.Clear();//重置购物车
            //断言
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //准备-创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" } }.AsQueryable());
            //准备-创建Cart
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(mock.Object,null);
            //动作-对cart添加一个产品
            target.AddToCart(cart, 1, null);
            //断言
            Assert.AreEqual(cart.Lines.ToArray().Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_T0_Cart_screen()
        {
            //准备-创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" } }.AsQueryable());
            //准备-创建Cart
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(mock.Object,null);
            //动作-对cart添加一个产品
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");
            //断言
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //准备-创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" } }.AsQueryable());
            //准备-创建Cart
            Cart cart = new Cart();
            //准备-创建控制器
            CartController target = new CartController(mock.Object,null);
            //动作-调用Index动作方法
            CartIndexViewModel index = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            //断言
            Assert.AreSame(index.Cart, cart);
            Assert.AreEqual(index.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor>mock=new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart=new Cart();
            //准备-创建一个送货地址实例
            ShippingDetails shippingDetails=new ShippingDetails();
            //准备-创建一个控制器实例
            CartController target=new CartController(null,mock.Object);
            ViewResult result=target.Checkout(cart,shippingDetails);
            //断言-检查，订单尚未传递给处理器
            mock.Verify(m=>m.ProcessOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never);
            //断言-检查，该方法返回的是默认视图
            Assert.AreEqual("",result.ViewName);
            //断言-检查，给视图传递的是一个非法模型
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart = new Cart();
            cart.AddItem(new Product(),1);
            //准备-创建一个送货地址实例
            ShippingDetails shippingDetails = new ShippingDetails();
            //准备-创建一个控制器实例
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error","error");
            ViewResult result = target.Checkout(cart, shippingDetails);
            //断言-检查，订单尚未传递给处理器
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);
            //断言-检查，该方法返回的是默认视图
            Assert.AreEqual("", result.ViewName);
            //断言-检查，给视图传递的是一个非法模型
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_Andu_Submit_Order()
        {
            //准备-创建一个模仿的订单处理器
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //准备-创建一个空的购物车
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //准备-创建一个送货地址实例
            ShippingDetails shippingDetails = new ShippingDetails();
            //准备-创建一个控制器实例
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);
            //断言-检查，订单已经传递给处理器
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once);
            //断言-检查，该方法返回的是"Completed"视图
            Assert.AreEqual("Completed", result.ViewName);
            //断言-检查，我们正在把一个有效的模型传递给视图
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
