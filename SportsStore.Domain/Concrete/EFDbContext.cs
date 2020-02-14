using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using System.Data.Entity;

namespace SportsStore.Domain.Concrete
{
    /// <summary>
    /// 该类的每个属性与数据库的数据表对应
    /// </summary>
    public class EFDbContext : DbContext
    {
        /// <summary>
        /// 属性名为数据表名
        /// </summary>
        public DbSet<Product> Products { get; set; }
    }
}
