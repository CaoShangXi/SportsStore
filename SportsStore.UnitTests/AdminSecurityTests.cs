using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.WebUI.Infrastructure.Abstract;
using Moq;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// AdminSecurityTests 的摘要说明
    /// </summary>
    [TestClass]
    public class AdminSecurityTests
    {

        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel { UserName = "admin", Password = "secret" };
            //准备-创建控制器
            AccountController target=new AccountController(mock.Object);
            //动作-使用合法凭据进行认证
            ActionResult result=target.Login(model,"/MyURL");
            //断言
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
            Assert.AreEqual("/MyURL",((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPassword")).Returns(false);
            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel { UserName = "badUser", Password = "badPassword" };
            //准备-创建控制器
            AccountController target = new AccountController(mock.Object);
            //动作-使用合法凭据进行认证
            ActionResult result = target.Login(model, "/MyURL");
            //断言
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
