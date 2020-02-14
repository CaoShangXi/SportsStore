using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Concrete;
using System.Configuration;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDenpendencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDenpendencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            //使用Moq创建模拟存储库
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product> {
            //new Product{Name="Football",Price=25},
            //new Product{Name="Surf board",Price=179},
            //new Product{Name="Running shoes",Price=95}
            //});
            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            //为接口指定实现类
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            //创建并初始化邮件参数配置对象
            EmailSettings emailSettings=new EmailSettings {
                //从Web.config文件name为Email.WriteAsFile的元素读取参数
                writeAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"]??"false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings",emailSettings);

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}