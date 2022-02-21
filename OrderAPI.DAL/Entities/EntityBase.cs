using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.DAL.Entities
{
    public class EntityBase
    {
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
