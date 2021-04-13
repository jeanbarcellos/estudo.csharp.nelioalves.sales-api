using System;

namespace SalesWebApi.Models.ViewModels
{
    public class SellerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        public int DepartmentId { get; set; }
    }
}