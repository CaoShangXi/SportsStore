using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.WebUI.Infrastructure.Abstract
{
    /// <summary>
    /// 身份认证提供器
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// 验证用户提供的凭据（即用户名和密码）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authenticate(string username,string password);
    }
}
