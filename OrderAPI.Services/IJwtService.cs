using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Services
{
    public interface IJwtService
    {
        public string GenerateJWT();
    }
}
