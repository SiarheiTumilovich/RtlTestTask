using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Domains
{
    [Table(nameof(ShowPersonAssoc))]
    public class ShowPersonAssoc
    {
        public int ShowId { get; set; }
        public int PersonId { get; set; }

        [ForeignKey(nameof(ShowId))]
        public virtual Show Show { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
    }
}
