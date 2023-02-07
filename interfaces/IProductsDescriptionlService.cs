using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface IProductsDescriptionlService
    {
        Task<IEnumerable<ProductDescription>> GetAll(int idProduct);
        Task<ProductDescription> GetByID(string ID);
        Task<(string errorMessage, List<string> imageName)> UploadImage(IFormFileCollection formFiles);
        Task Create(ProductDescription productDescription, List<string> imageName);
        Task DeleteImage(string fileName);
    }
}
