using Microsoft.AspNetCore.Identity;

namespace ChefManager.Server.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string InternalPassword { get; set; } = "";

        public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
        public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();


    }
}
