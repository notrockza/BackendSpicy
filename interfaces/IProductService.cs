using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetByID(int id);
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(Product product);

        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);

        Task DeleteImage(string flieName);
    }
}
