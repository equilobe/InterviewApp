using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestDelivery.UI.ViewModels
{
    public class Playground
    {
        public List<string> Names { get; set; }

        public List<Item> Items { get; set; }
        
    }

    public class Item
    {
        public string Name { get; set; }

        public List<ItemDetails> Details { get; set; }
    }

    public class ItemDetails
    {
        public string Name { get; set; }
    }
}