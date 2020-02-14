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
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1"},
                    new Product{ProductID=2,Name="P2"},
                    new Product{ProductID=3,Name="P3"},
                    new Product{ProductID=4,Name="P4"},
                    new Product{ProductID=5,Name="P5"}
                }
                );
            //创建并初始化Controller对象
            ProductController controller = new ProductController(mock.Object);
            //设置页面要显示的产品数
            controller.pageSize = 3;
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;
            //断言
            Product[] prodArray = result.Products.ToArray();//转换成数组
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");//比较实际的值与预言的值是否相同
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        /// <summary>
        /// 测试能否生成链接按钮
        /// </summary>
        [TestMethod]
        public void Can_Generate_page_Links()
        {
            //准备-定义一个HTML辅助器，这是必须的，目的是运用扩展方法
            HtmlHelper myHelper = null;
            //准备-创建PagingInfo数据
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPrePage = 10
            };
            //准备-用lambda表达式建立委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //动作
            MvcHtmlString result=myHelper.PageLinks(pagingInfo,pageUrlDelegate);
            //断言
            Assert.AreEqual(
                @"<a class=""btn btn-default"" href=""Page1"">1</a><a class=""btn btn-default btn-primary Selected"" href=""Page2"">2</a><a class=""btn btn-default"" href=""Page3"">3</a>"
                , result.ToString());
        }

        /// <summary>
        /// 测试分页数据是否正确
        /// </summary>
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1"},
                    new Product{ProductID=2,Name="P2"},
                    new Product{ProductID=3,Name="P3"},
                    new Product{ProductID=4,Name="P4"},
                    new Product{ProductID=5,Name="P5"}
                }
                );
            //创建并初始化Controller对象
            ProductController controller = new ProductController(mock.Object);
            //设置页面要显示的产品数
            controller.pageSize = 3;
            ProductsListViewModel result=(ProductsListViewModel)controller.List(null,2).Model;
            //断言
            PagingInfo pagingInfo=result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage,2);
            Assert.AreEqual(pagingInfo.ItemsPrePage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPage, 2);
        }
        
        /// <summary>
        /// 测试能否对产品列表分类
        /// </summary>
        [TestMethod]
        public void Can_Filter_Products()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1",Category="cat1"},
                    new Product{ProductID=2,Name="P2",Category="cat2"},
                    new Product{ProductID=3,Name="P3",Category="cat1"},
                    new Product{ProductID=4,Name="P4",Category="cat2"},
                    new Product{ProductID=5,Name="P5",Category="cat3"}
                }
                );
            //创建并初始化Controller对象
            ProductController controller = new ProductController(mock.Object);
            //设置页面要显示的产品数
            controller.pageSize = 3;
            Product[] result = ((ProductsListViewModel)controller.List("cat2", 1).Model).Products.ToArray();
            //断言
            Assert.AreEqual(result.Length,2);
            Assert.IsTrue(result[0].Name=="P2"&&result[0].Category=="cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "cat2");
        }

        /// <summary>
        /// 测试能否分类
        /// </summary>
        [TestMethod]
        public void Can_Create_Categories()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1",Category="Apples"},
                    new Product{ProductID=2,Name="P2",Category="Apples"},
                    new Product{ProductID=3,Name="P3",Category="Plums"},
                    new Product{ProductID=4,Name="P4",Category="Oranges"},
                }
                );
            //创建并初始化Controller对象
            NavController target=new NavController(mock.Object);
            string [] results=((IEnumerable<string>)target.Menu().Model).ToArray();
            //断言
            Assert.AreEqual(results.Length,3);
            Assert.AreEqual(results[0],"Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        /// <summary>
        /// 测试选中的分类
        /// </summary>
        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1",Category="Apples"},
                    new Product{ProductID=2,Name="P2",Category="Oranges"},
                }
                );
            //创建并初始化Controller对象
            NavController target = new NavController(mock.Object);
            //定义已选分类
            string categoryToSelect = "Apples";
            //动作
            string result=target.Menu(categoryToSelect).ViewBag.SelectedCategory;
            //断言
            Assert.AreEqual(categoryToSelect,result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //模拟创建IProductRepository接口实现类的实例
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //为实例的Products属性初始化产品数据
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductID=1,Name="P1",Category="cat1"},
                    new Product{ProductID=2,Name="P2",Category="cat2"},
                    new Product{ProductID=3,Name="P3",Category="cat1"},
                    new Product{ProductID=4,Name="P4",Category="cat2"},
                    new Product{ProductID=5,Name="P5",Category="cat3"}
                }
                );
            //创建并初始化Controller对象
            ProductController target = new ProductController(mock.Object);
            //设置页面要显示的产品数
            target.pageSize = 3;
            //动作-测试不同分类的产品数
            int result1=((ProductsListViewModel)target.List("cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)target.List("cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)target.List("cat3").Model).PagingInfo.TotalItems;
            int resultAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;
            //断言
            Assert.AreEqual(result1, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
            Assert.AreEqual(resultAll, 5);
        }
    }
}
