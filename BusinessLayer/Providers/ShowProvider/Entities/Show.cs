using System.Collections.Generic;

namespace BusinessLayer.Providers.ShowProvider.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Person> People { get; set; }
    }
}
