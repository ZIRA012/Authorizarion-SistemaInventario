using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SI.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required string Genre { get; set; }

        public decimal Price { get; set; }

        public DateOnly ReleaseDate { get; set; }
    }
}
