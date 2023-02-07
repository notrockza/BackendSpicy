using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAll(int idAccount);
        Task<Cart> GetByID(string ID);
        Task Create(Cart cart);
        Task Update(Cart cart);
        Task Delete(Cart cart);
    }
}
