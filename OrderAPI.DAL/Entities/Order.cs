using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrderAPI.DAL.Entities
{
    public class Order: EntityBase
    {
        [Key]
        public string OrderId { get; set; }

        public List<ProductTypeQuantity> ProductTypesQuantities { get; set; } = new List<ProductTypeQuantity>();

        public decimal RequiredBinWidth { get; set; }

        [ForeignKey("CustomerId")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
