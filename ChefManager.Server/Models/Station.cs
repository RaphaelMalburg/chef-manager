using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChefManager.Server.Models
{
    /// <summary>
    /// Represents a storage place entity.
    /// </summary>
    public class Station
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public virtual ICollection<StoragePlace> StoragePlaces { get; set; } = new List<StoragePlace>();

    }
}
