using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlackjackDBLib
{
    public class BlackjackDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<Turn> Turns { get; set; }
    }
}
