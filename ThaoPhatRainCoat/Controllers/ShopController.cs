using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raincoat.BUS;
using Raincoat.BE;
namespace ThaoPhatRainCoat.Controllers
{
    public class ShopController : Controller
    {
        private ShopBUS shopBUS = new ShopBUS();
        // GET: Shop
        public ActionResult Index()
        {
            List<ProductsBE> prodList = this.shopBUS.GetAllProducts();
            return View();
        }
    }
}