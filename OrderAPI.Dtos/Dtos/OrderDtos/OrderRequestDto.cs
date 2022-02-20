using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderAPI.Dtos.Dtos.OrderDtos
{
    public class OrderRequestDto
    {
        [Required(ErrorMessage = "OrderId is Required"), MaxLength(30)]
        public string OrderId { get; set; }
        [Required(ErrorMessage = "CustomerId is Required"), MaxLength(30)]
        public string CustomerId { get; set; }
        [Required(ErrorMessage = "ProductTypesQuantities is Required")]
        public List<ProductTypeQuantityDto> ProductTypesQuantities { get; set; } = new List<ProductTypeQuantityDto>();

    }
}
