using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using System.Collections.Generic;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //准备-创建一个带有图像数据的Product
            Product prod=new Product {ProductID=2,Name="Test",ImageData=new byte[] { },ImageMimeType="image/png" };
            //准备-创建模仿存储库
            Mock<IProductRepository>mock=new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[] { 
            new Product{ProductID=1,Name="P1"},
            prod,
            new Product{ProductID=3,Name="P2"}
            }.AsQueryable());
            //准备-创建控制器
            ProductController target=new ProductController(mock.Object);
            //动作-调用GetImage方法
            ActionResult result=target.GetImage(2);
            //断言
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType,((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            //准备-创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();//创建模仿对象
            mock.Setup(m => m.Products)//用Setup方法建立模仿对象的属性
            .Returns(new Product[] {
            new Product{ProductID=1,Name="P1"},
            new Product{ProductID=3,Name="P2"}
            }.AsQueryable());//用Return方法指定属性的返回类型，并初始化数据
            //准备-创建控制器
            ProductController target = new ProductController(mock.Object);
            //动作-调用GetImage方法
            ActionResult result = target.GetImage(2);
            //断言
            Assert.IsNull(result);
        }
    }
}
