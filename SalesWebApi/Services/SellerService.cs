using SalesWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Services.Exceptions;
using SalesWebApi.Data;

namespace SalesWebApi.Services
{
    public class SellerService
    {
        private readonly SalesWebApiContext _context;

        public SellerService(SalesWebApiContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindAsync(int id)
        {
            // return await _context.Seller.FindAsync(id);
            return await _context.Seller.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
            // return await _context.Seller.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);

                if (obj == null)
                {
                    throw new NotFoundException("Id not found");
                }

                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Não é possível excluir o vendedor porque ele / ela tem vendas
                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        public bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
    }
}