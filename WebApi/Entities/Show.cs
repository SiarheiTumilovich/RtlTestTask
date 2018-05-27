using System.Collections.Generic;

namespace WebApi.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Person> Cast { get; set; }
    }
}
