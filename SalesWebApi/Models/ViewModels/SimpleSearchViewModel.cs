using SalesWebApi.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesWebApi.Models.ViewModels
{
    public class SimpleSearchViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Amount { get; set; }
        public string SellerName { get; set; }
        public string DepartamentName { get; set; }
        public SaleStatus Status { get; set; }
    }
}