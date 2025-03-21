using Microsoft.AspNetCore.Identity;

namespace progetto_settimanale_S18.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
