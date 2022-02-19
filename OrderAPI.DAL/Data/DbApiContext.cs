using Microsoft.EntityFrameworkCore;
using OrderAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAPI.DAL.Data
{
    public class DbApiContext : DbContext, IDbApiContext
    {

        public DbApiContext(DbContextOptions<DbApiContext> options)
          : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductTypeQuantity> ProductTypesQuantities { get; set; }

    }
}
