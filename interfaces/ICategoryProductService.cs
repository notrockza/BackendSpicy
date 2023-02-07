using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface ICategoryProductService
    {
        Task<IEnumerable<CategoryProduct>> GetAll();
    }
}
