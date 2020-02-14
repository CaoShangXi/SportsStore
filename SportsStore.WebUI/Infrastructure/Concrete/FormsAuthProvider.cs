using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SportsStore.WebUI.Infrastructure.Abstract;

namespace SportsStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result=FormsAuthentication.Authenticate(username,password);
            if (result)
            {
                //该方法对浏览器的响应添加一个cookie，之后不需要对用户的每个请求进行验证
                FormsAuthentication.SetAuthCookie(username,false);
            }
            return result;
        }
    }
}