using SalesWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Data;

namespace SalesWebApi.Services
{
    public class DepartmentService
    {
        private readonly SalesWebApiContext _context;

        public DepartmentService(SalesWebApiContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}