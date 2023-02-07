using BackendSpicy.interfaces;
using BackendSpicy.Models;

namespace BackendSpicy.Services
{
    public class RoleService : IRoleService
    {

        private readonly DatabaseContext databaseContext;

        public RoleService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Role> GetRoleByID(int Id)
        {
            return await databaseContext.Role.FindAsync(Id);
        }
    }
}
