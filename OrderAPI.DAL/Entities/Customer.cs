using OrderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderAPI.DAL.Entities
{
    public class Customer: EntityBase
    {
        [Key]
        public string CustomerId { get; set; } = UUIDGenerator.GetNewUUID();

        [Required(ErrorMessage = "UserName is Required"), MaxLength(30)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required"), MaxLength(30)]
        public string Password { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
