using System.Collections.Generic;

namespace SalesWebApi.Models.ViewModels
{
    public class GroupingSearchViewModel
    {
        public Department Department { get; set; }
        public List<SalesRecord> Items { get; set; }
        public double Total { get; set; }
    }
}