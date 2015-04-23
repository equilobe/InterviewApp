using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.ServiceLayer.Lists
{
    public class Page<T> : TestDelivery.ServiceLayer.Lists.IPage 
    {
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int CurrentPage { get; set; }

        public int StartItemPosition { get; set; }
        public int EndItemPosition { get; set; }
        public int TotalItemsCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
