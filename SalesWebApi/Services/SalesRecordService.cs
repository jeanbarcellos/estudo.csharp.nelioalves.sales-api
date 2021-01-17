using SalesWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Data;
using SalesWebApi.Models.ViewModels;

namespace SalesWebApi.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebApiContext _context;

        public SalesRecordService(SalesWebApiContext context)
        {
            _context = context;
        }

        public async Task<List<SimpleSearchViewModel>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .Select(x => ItemToDto(x))
                .ToListAsync();
        }

        public SimpleSearchViewModel ItemToDto(SalesRecord x)
        {
            return new SimpleSearchViewModel
            {
                Id = x.Id,
                Date = x.Date,
                Amount = x.Amount,
                Status = x.Status,
                SellerName = x.Seller.Name,
                DepartamentName = x.Seller.Department.Name
            };
        }

        public async Task<List<GroupingSearchViewModel>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            DateTime minDateTime = DateTime.Parse(minDate.Value.ToString("yyyy-MM-dd"));
            DateTime maxDateTime = DateTime.Parse(maxDate.Value.ToString("yyyy-MM-dd"));

            // #TODO# Resolver problema...
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .Select(group => new GroupingSearchViewModel
                {
                    Department = group.Key,
                    Items = group.ToList(),
                    Total = group.Key.TotalSales(minDateTime, maxDateTime),
                })
                .ToListAsync();
        }
    }
}