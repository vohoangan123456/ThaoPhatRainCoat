using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raincoat.BE
{
    public class ProductsBE
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}