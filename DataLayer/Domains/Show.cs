using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Domains
{
    [Table(nameof(Show))]
    public class Show
    {
        [Key]
        public int ShowId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ShowPersonAssoc> People { get; set; }
    }
}
