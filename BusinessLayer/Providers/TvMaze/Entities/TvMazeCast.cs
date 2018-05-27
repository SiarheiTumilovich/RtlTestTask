using System;

namespace BusinessLayer.Providers.TvMaze.Entities
{
    internal class TvMazeCast
    {
        public class TvMazeCastCharacter
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class TvMazeCastPerson
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? Birthday { get; set; }
        }

        public TvMazeCastCharacter Character { get; set; }
        public TvMazeCastPerson Person { get; set; }
    }
}
