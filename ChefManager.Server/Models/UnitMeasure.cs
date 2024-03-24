using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChefManager.Server.Models
{
    /// <summary>
    /// Represents a storage place entity.
    /// </summary>
    public class UnitMeasure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";

        // Navigation property for one-to-many relationship with PreplistItem
        public virtual ICollection<PreplistItem> PreplistItems { get; set; } = new List<PreplistItem>();
    }
}