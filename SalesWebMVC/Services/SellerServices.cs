using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class SellerServices
    {
        private readonly SalesWebMVCContext _context;

        public SellerServices(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Seller seller)
        {
            _context.Update(seller);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Seller seller)
        {
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SellerExistsAsync(int id)
        {
            return await (_context.Seller?.AnyAsync(e => e.Id == id));
        }
    }
}
