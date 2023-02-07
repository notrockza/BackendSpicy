using BackendSpicy.Models;

namespace BackendSpicy.interfaces
{
    public interface IRoleService
    {
        Task<Role> GetRoleByID(int Id);
    }
}
