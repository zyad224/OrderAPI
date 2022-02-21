using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Dtos.Dtos.OrderDtos
{
    public class ProductTypeQuantityDto
    {
        public ProductTypeDto ProductType { get; set; }
        public int Quantity { get; set; }

    }
}
