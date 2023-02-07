using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Services
{
    public class CategoryProductService : ICategoryProductService
    {
        private readonly DatabaseContext databaseContext;

        public CategoryProductService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<CategoryProduct>> GetAll()
        {
            return await databaseContext.CategoryProduct.ToListAsync();
        }

    }
}
