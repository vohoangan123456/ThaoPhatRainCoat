﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raincoat.BUS;
using Raincoat.BE;
using ThaoPhatRainCoat.Models;
namespace ThaoPhatRainCoat.Controllers
{
    public class ShopController : Controller
    {
        private ShopBUS shopBUS = new ShopBUS();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllProducts()
        {
            var model = new ShopModel();
            List<ProductsBE> prodList = this.shopBUS.GetAllProducts();
            model.products = prodList;
            return Json(model, JsonRequestBehavior.AllowGet);
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
    }
}