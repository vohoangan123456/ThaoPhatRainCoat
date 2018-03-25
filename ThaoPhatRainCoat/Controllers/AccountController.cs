using ThaoPhatRainCoat.Utils;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace ThaoPhatRainCoat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Helpers.SetUserToken(null);
            return RedirectToAction("Index", "Shop");
        }

        public JsonResult Login(string account, string password)
        {
            var accountBase = System.Configuration.ConfigurationManager.AppSettings["Account"];
            var passwordBase = System.Configuration.ConfigurationManager.AppSettings["Password"];
            var returnString = false;
            if (accountBase.Equals(account) && GetMD5(password).Equals(passwordBase))
            {
                string token = Helpers.RandomString(20);
                Helpers.SetUserToken(token);
                returnString = true;
            }
            return Json(new
            {
                isSuccess = returnString,
                token = Helpers.GetUserToken()
            }, JsonRequestBehavior.AllowGet);
        }

        private string GetMD5(string chuoi)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }
    }
}