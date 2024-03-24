using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefManager.Server.Models
{
    /// <summary>
    /// Represents a storage place entity.
    /// </summary>
    public class Preplist
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public virtual ICollection<PreplistItem> PreplistItems { get; set; } = new List<PreplistItem>();
    }
}
