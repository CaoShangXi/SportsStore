using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    /// <summary>
    /// 邮件参数设置类，保存邮件设置的参数
    /// </summary>
    public class EmailSettings
    {
        public string mailToAddress = "orders@example.com";
        public string mailFromAddress = "Sportsstore@example.com";
        public bool UseSsl = true;
        public string username = "MySmtpUsername";
        public string password = "MySmtpPassword";
        public string serverName = "smtp.example.com";
        public int serverPort = 587;
        public bool writeAsFile = false;
        public string fileLocation = @"c:\sports_store_emails";
    }
}
