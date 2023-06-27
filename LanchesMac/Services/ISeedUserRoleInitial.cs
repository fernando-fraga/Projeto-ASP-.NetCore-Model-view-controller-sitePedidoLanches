using Microsoft.EntityFrameworkCore.Query.Internal;

namespace LanchesMac.Services
{
    public interface ISeedUserRoleInitial
    {
        void SeedRoles();
        
        void SeedUsers();
    }
}
