using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChefManager.Server.Models
{
    /// <summary>
    /// Represents a storage place entity.
    /// </summary>
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public virtual ICollection<MenuItem> Preplists { get; set; } = new List<MenuItem>();
    }
}
