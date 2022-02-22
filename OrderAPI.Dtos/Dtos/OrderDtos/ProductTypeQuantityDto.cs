using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderAPI.Dtos.Dtos.OrderDtos
{
    public class ProductTypeQuantityDto
    {
        public ProductTypeDto ProductType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter postive value")]
        public int Quantity { get; set; }

    }
}
