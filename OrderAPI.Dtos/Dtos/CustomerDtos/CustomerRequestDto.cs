using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Dtos.CustomerDtos
{
    public class CustomerRequestDto
    {
        [Required(ErrorMessage = "UserName is Required"), MaxLength(30)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required"), MaxLength(30)]
        public string Password { get; set; }
    }
}
