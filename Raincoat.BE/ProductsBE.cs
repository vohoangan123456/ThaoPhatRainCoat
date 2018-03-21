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
        public int Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool isEditable { get; set; }
        public string ImageValue { get; set; }

        public ProductsBE()
        {
            Id = -1;
            Name = string.Empty;
            Description = string.Empty;
            Price = 0;
            CreatedDate = DateTime.Today;
            CreatedBy = string.Empty;
            isEditable = false;
            ImageValue = string.Empty;
        }
    }
}