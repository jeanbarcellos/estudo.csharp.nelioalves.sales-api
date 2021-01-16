using SalesWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Data;
using SalesWebApi.Services.Exceptions;

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

        public async Task<Department> FindAsync(int id)
        {
            // return await _context.Department.FindAsync(id);
            return await _context.Department.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertAsync(Department obj)
        {
            _context.Add(obj);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department obj)
        {
            bool hasAny = await _context.Department.AnyAsync(x => x.Id == obj.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            _context.Update(obj);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Department.FindAsync(id);

            if (obj == null)
            {
                throw new NotFoundException("Id not found");
            }

            _context.Department.Remove(obj);

            await _context.SaveChangesAsync();
        }

        public bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);

        }


    }
}