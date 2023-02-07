using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAll();
        Task<object> Register(Account account);
        Task<Account> Login(string email, string password);

        string GenerateToken(Account account);
        Account GetInfo(string accessToken);

        Task<Account> GetByID(int id);
        Task UpdateAccount(Account account);
        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);

        Task DeleteImage(string fileName);



    }
}
