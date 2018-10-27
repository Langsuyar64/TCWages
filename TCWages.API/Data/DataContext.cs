using System;
using Microsoft.EntityFrameworkCore;
using TCWages.API.Models;

namespace TCWages.API.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) {

        }
        public DbSet<Value> Values {
            get;
            set;
        }
    }
}