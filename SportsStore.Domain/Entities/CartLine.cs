using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    /// <summary>
    /// 该类表示用户所选的产品和购买的数量
    /// </summary>
    public class CartLine
    {
        public Product Product { get; set; }//产品
        public int Quantity { get; set; }//产品数量
    }
}
