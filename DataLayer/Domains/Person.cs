using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Domains
{
    [Table(nameof(Person))]
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
