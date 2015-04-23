using System;
namespace TestDelivery.ServiceLayer.Lists
{
    public interface IPage
    {
        int CurrentPage { get; set; }
        int EndItemPosition { get; set; }
        bool HasNextPage { get; set; }
        bool HasPreviousPage { get; set; }
        int StartItemPosition { get; set; }
        int TotalItemsCount { get; set; }
    }
}
