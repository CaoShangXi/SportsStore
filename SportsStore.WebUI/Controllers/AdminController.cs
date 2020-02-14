using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]//授权过滤器注解，如果用户没有认证，则被重定向到web.config表单的authentication元素所指定的URL
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        // GET: Admin
        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        /// <summary>
        /// 保存用户产品信息的修改
        /// </summary>
        /// <param name="product">模型绑定器绑定用户递交的数据形成Product对象，并将该对象传递给Edit方法，如果一系列正常，则ModelState.IsValid为真</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            /*如果通过Product类的注解验证*/
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                //保存临时消息，在HTTP请求结束时被删除
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        /// <summary>
        /// 添加新产品
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productID)
        {
            Product deleteProduct = repository.DeleteProduct(productID);
            if (deleteProduct != null)
            {
                //保存临时消息，在HTTP请求结束时被删除
                TempData["message"] = string.Format("{0} was deleted", deleteProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}