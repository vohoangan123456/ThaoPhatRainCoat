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

        public ShopModel()
        {
            products = new List<ProductsBE>();
        }
    }
}