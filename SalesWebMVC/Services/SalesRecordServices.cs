using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace SalesWebMVC.Services
{
    public class SalesRecordServices
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordServices(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<SalesRecord> FindAll()
        {
            return _context.SalesRecord.ToList();
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date > minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date < maxDate.Value);
            }
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
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
            var res = await result
            .Include(x => x.Seller)
            .Include(x => x.Seller.Department)
            .OrderBy(x => x.Date)
            .ToListAsync();
            return res.GroupBy(x => x.Seller.Department).ToList();
        }
    }
}
