using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.ServiceLayer.Lists
{
    public class PageInfo
    {
        public PageInfo()
        {
            Page = 1;
            PageSize = 7;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
