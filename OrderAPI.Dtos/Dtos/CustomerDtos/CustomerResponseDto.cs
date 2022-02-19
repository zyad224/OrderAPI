using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Dtos.CustomerDtos
{
    public class CustomerResponseDto
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }
    }
}
