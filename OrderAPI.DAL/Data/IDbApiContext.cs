using Microsoft.EntityFrameworkCore;
using OrderAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.DAL.Data
{
    public interface IDbApiContext
    {
        public DbSet<Customer> Customers { get; set;}
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductTypeQuantity> ProductTypesQuantities { get; set; }
    }
}
