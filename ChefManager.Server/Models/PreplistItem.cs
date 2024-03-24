using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChefManager.Server.Models
{
    /// <summary>
    /// Represents a storage place entity.
    /// </summary>
    public class PreplistItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Amount { get; set; }

        // Navigation property for many-to-one relationship with Preplist
        public int PreplistId { get; set; }
        public virtual Preplist? Preplist { get; set; }

        // Navigation property for many-to-one relationship with UnitMeasure
        public int UnitMeasureId { get; set; }
        public virtual UnitMeasure? UnitMeasure { get; set; }
    }
}

