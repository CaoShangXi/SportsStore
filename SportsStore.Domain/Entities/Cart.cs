using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    /// <summary>
    /// 购物车类
    /// </summary>
    public class Cart
    {
        List<CartLine> lineCollection=new List<CartLine>();

        /// <summary>
        /// 添加物品到购物车
        /// </summary>
        /// <param name="product">物品</param>
        /// <param name="quantity">物品数量</param>
        public void AddItem(Product product,int quantity)
        {
            CartLine line = lineCollection
                .Where(p=>p.Product.ProductID==product.ProductID)
                .FirstOrDefault();
            if (line==null)
            {
                //添加物品
                lineCollection.Add(new CartLine { Product=product,Quantity=quantity});
            }
            else
            {
                line.Quantity += quantity;//更新物品数量
            }
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="product">物品</param>
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(x=>x.Product.ProductID==product.ProductID);
        }

        /// <summary>
        /// 计算总额
        /// </summary>
        /// <returns>物品总额</returns>
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(x=>x.Product.Price*x.Quantity);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        public void Clear()
        {
            lineCollection.Clear();
        }

        /// <summary>
        /// 获取购物车所有物品信息
        /// </summary>
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
}
