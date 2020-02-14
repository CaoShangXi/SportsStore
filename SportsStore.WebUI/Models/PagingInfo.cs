using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    /// <summary>
    /// 视图模型，使用此类可将可用页面数、当前页、产品总数的信息传递给视图
    /// </summary>
    public class PagingInfo
    {
        public int TotalItems { get; set; }//总产品数量
        public int ItemsPrePage { get; set; }//每页容纳的产品数量
        public int CurrentPage { get; set; }//当前页
        public int TotalPage//总页数
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPrePage); }
        }
    }
}