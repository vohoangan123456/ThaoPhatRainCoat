using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raincoat.BE
{
    public class SlideshowBE
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageValue { get; set; }
        public int Order { get; set; }

        public SlideshowBE()
        {
            Id = 0;
            Name = string.Empty;
            ImageValue = string.Empty;
            Order = 0;
        }
    }
}