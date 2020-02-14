using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    /// <summary>
    /// 订单处理接口，对订单的细节进行处理
    /// </summary>
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart,ShippingDetails shippingDetails);
    }
}
