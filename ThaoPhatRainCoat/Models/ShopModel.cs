using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raincoat.BE;
namespace ThaoPhatRainCoat.Models
{
    public class ShopModel
    {
        public List<ProductsBE> products { get; set; }
        public List<SlideshowBE> slideshows { get; set; }
        public ProductsBE productBE { get; set; }
        public SlideshowBE slideBE { get; set; }

        public ShopModel()
        {
            products = new List<ProductsBE>();
            slideshows = new List<SlideshowBE>();
            productBE = new ProductsBE();
            slideBE = new SlideshowBE();
        }
    }
}