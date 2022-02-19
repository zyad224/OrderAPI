using OrderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrderAPI.DAL.Entities
{
    public class ProductTypeQuantity: EntityBase
    {
        [Key]
        public string ProductTypeQuantityId { get; set; } = UUIDGenerator.GetNewUUID();

        public ProductType ProductType { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("OrderId")]
        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
