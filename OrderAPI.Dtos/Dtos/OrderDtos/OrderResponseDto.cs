using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Dtos.Dtos.OrderDtos
{
    public class OrderResponseDto
    {
        public List<ProductTypeQuantityDto> ProductTypesQuantities { get; set; }
        public decimal RequiredBinWidth { get; set; }

    }
}
