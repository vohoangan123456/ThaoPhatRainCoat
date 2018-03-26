using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raincoat.BUS;
using Raincoat.BE;
using ThaoPhatRainCoat.Models;
using ThaoPhatRainCoat.Utils;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

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
            List<ProductsBE> prodList = this.shopBUS.GetAllProducts(GetHostPath());
            model.products = prodList;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByCondition(string searchKey, string orderBy)
        {
            int orderByInt = int.Parse(orderBy);
            List<ProductsBE> productList = this.shopBUS.GetProductByCondition(searchKey, orderByInt, GetHostPath());
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult CreateNewProduct(ProductsBE product)
        {
            product.hostPath = GetHostPath();
            int productId = this.shopBUS.CreateNewProduct(product);
            UploadImage(product.ImageValue, productId, "products");
            product.Id = productId;
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateProduct(ProductsBE product)
        {
            product.hostPath = GetHostPath();
            UploadImage(product.ImageValue, product.Id, "products");
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
            List<SlideshowBE> slideList = this.slideBUS.GetAllSlides(GetHostPath());
            model.slideshows = slideList;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewSlide(SlideshowBE slide)
        {
            slide.hostPath = GetHostPath();
            int slideId = this.slideBUS.CreateNewSlide(slide);
            UploadImage(slide.ImageValue, slideId, "slides");
            slide.Id = slideId;
            return Json(slide, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSlide(SlideshowBE slide)
        {
            slide.hostPath = GetHostPath();
            UploadImage(slide.ImageValue, slide.Id, "slides");
            bool isSuccess = this.slideBUS.UpdateSlide(slide);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSlide(int slideId)
        {
            bool isSuccess = this.slideBUS.DeleteSlide(slideId);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageValue"></param>
        /// <param name="Id"></param>
        /// <param name="folderName">products, slides</param>
        private void UploadImage(string ImageValue, int Id, string folderName)
        {
            var base64Data = Regex.Match(ImageValue, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            using (var stream = new MemoryStream(binData))
            {
                var x = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                Bitmap Image = new Bitmap(stream);
                Image.Save(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("Content/images/{0}/{1}.jpg", folderName, Id)));
            }
        }
        private string GetHostPath()
        {
            UriBuilder builder = null;
            builder = new UriBuilder(Request.Url.Scheme,
                                            Request.Url.Host,
                                            Request.Url.Port);
            return builder.ToString();
        }
    }
}