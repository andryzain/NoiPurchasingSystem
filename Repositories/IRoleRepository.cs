using Microsoft.AspNetCore.Identity;

namespace NoiPurchasingSystem.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
