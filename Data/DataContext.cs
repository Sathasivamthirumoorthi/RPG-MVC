using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ) : base(options)
        {
            
        }

        // Creates a DbSet<TEntity> that can be used to query and save instances of Character.
        public DbSet<Character> Characters {get; set;}
        public DbSet<User> Users {get; set;}

    }
}