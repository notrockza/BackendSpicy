using BackendSpicy.interfaces;
using BackendSpicy.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Services
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext databaseContext;

        public CartService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task Create(Cart cart)
        {
            var result = await databaseContext.Cart.AsNoTracking().FirstOrDefaultAsync(e => e.ProductID == cart.ProductID && e.AccountID == cart.AccountID);
            if (result != null)
            {
                result.AmountProduct += cart.AmountProduct;
                databaseContext.Cart.Update(result);
                await databaseContext.SaveChangesAsync();
            }
            else
            {
                if (string.IsNullOrEmpty(cart.ID))
                {
                    cart.ID = GenerateID();
                }
                await databaseContext.Cart.AddAsync(cart);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task Delete(Cart cart)
        {
            databaseContext.Remove(cart);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cart>> GetAll(int idAccount)
        {
            return await databaseContext.Cart.Include(e => e.Product).Include(j => j.Product.CategoryProduct).Where(a => a.AccountID.Equals(idAccount)).ToListAsync();
        }

        public async Task<Cart> GetByID(string ID)
        {
            var result = await databaseContext.Cart.Include(e => e.Product).Include(j => j.Product.CategoryProduct).AsNoTracking().FirstOrDefaultAsync(e => e.ID == ID);
            if (result == null)
            {
                throw new Exception("ไม่พบสินค้า");
            }
            return result;
        }

        public Task Update(Cart cart)
        {
            throw new NotImplementedException();
        }

        public string GenerateID()
        {
            var result = databaseContext.Cart.OrderByDescending(a => a.ID).FirstOrDefault();
            if (result != null)
            {
                int ID = Convert.ToInt32(result.ID);
                return (ID + 1).ToString();
            }
            return "1";
        }
    }
}
