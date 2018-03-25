using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raincoat.BUS;
using Raincoat.BE;
using ThaoPhatRainCoat.Models;
using ThaoPhatRainCoat.Utils;

namespace ThaoPhatRainCoat.Controllers
{
    public class ShopController : Controller
    {
        private ShopBUS shopBUS = new ShopBUS();
        private SlideshowBUS slideBUS = new SlideshowBUS();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SlideShow()
        {
            string token = Helpers.GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return View("Index");
            }
            return View("slideshow");
        }
        #region Products
        public JsonResult GetAllProducts()
        {
            var model = new ShopModel();
            List<ProductsBE> prodList = this.shopBUS.GetAllProducts();
            model.products = prodList;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByCondition(string searchKey, string orderBy)
        {
            int orderByInt = int.Parse(orderBy);
            List<ProductsBE> productList = this.shopBUS.GetProductByCondition(searchKey, orderByInt);
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewProduct(ProductsBE product)
        {
            int productId = this.shopBUS.CreateNewProduct(product);
            return Json(productId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateProduct(ProductsBE product)
        {
            bool isSuccess = this.shopBUS.UpdateProduct(product);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProduct(int productId)
        {
            bool isSuccess = this.shopBUS.DeleteProduct(productId);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Slide
        public JsonResult GetAllSlides()
        {
            var model = new ShopModel();
            List<SlideshowBE> slideList = this.slideBUS.GetAllSlides();
            model.slideshows = slideList;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewSlide(SlideshowBE slide)
        {
            int slideId = this.slideBUS.CreateNewSlide(slide);
            return Json(slideId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSlide(SlideshowBE slide)
        {
            bool isSuccess = this.slideBUS.UpdateSlide(slide);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSlide(int slideId)
        {
            bool isSuccess = this.slideBUS.DeleteSlide(slideId);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}