using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CPSC571Project6.Models
{
    public class ConnectionDBClass : DbContext
    {
        public ConnectionDBClass(DbContextOptions<ConnectionDBClass> options) : base(options)
        {

        }

        public DbSet<TestClass> Table_1 { get; set; }

        public DbSet<TopicClass> Topics { get; set; }
    }
}
