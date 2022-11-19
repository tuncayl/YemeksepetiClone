using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tamircisepeti22.Models;

namespace Tamircisepetiv2.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        
        TmrsepetiEntities db=new TmrsepetiEntities();
        // GET: Security

        public ActionResult login()
        {
            
            return View();
        }
        [HttpPost]
       
        public ActionResult login(user user)
        {
            var userIndb = db.users.FirstOrDefault(x => x.UKullaniciad == user.UKullaniciad && x.Upassword == user.Upassword);
            if(userIndb != null && userIndb.Uauthid==0)
            {
                FormsAuthentication.SetAuthCookie(user.UKullaniciad, false);

                HttpCookie cookie = new HttpCookie("Kullancibilgi");
                cookie["isim"] = userIndb.Uname;
                cookie["soyisim"] = userIndb.USurname;
                cookie["eposta"] = userIndb.Uemail;
                cookie["tarih"] = userIndb.Utarih.ToString();
                cookie["id"] = userIndb.Uid.ToString();
                Response.Cookies.Add(cookie);
                Session.Remove("control");
                return RedirectToAction("Index", "Home");

            }
            else
            {
                
                return View();
            }
        }
        public ActionResult logout()
        {
            HttpContext.Response.Cookies["Kullancibilgi"].Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();
            
            return RedirectToAction("login");
        }
        [HttpPost]
        public JsonResult ililce(int? ilID, string tip)
        {
            TmrsepetiEntities db = new TmrsepetiEntities();
            List<SelectListItem> sonuc = new List<SelectListItem>();

            bool basariliMi = true;
            try
            {
                switch (tip)
                {
                    case "ililceGetir":
                        foreach (var ilce in db.Ilcelers.Where(il => il.IlID == 20).ToList())
                        {
                            foreach (var ilceisim in db.Ilcelers.Where(isim => isim.IlceID == ilce.IlceID).ToList())
                            {

                                foreach (var semt in db.tCity_District_Street_Town.Where(mah => mah.ilce == ilceisim.Ilce.ToUpper().ToString()).ToList()) //yordum biraz
                                {

                                    sonuc.Add(new SelectListItem
                                    {
                                        Text = ilceisim.Ilce.ToString() + "  (" + semt.mahalle + ")",
                                        Value = semt.id.ToString()
                                    });

                                }

                            }
                        }


                        break;


                    default:
                        break;

                }
            }
            catch (Exception)
            {
                //hata ile karşılaşırsak buraya düşüyor
                basariliMi = false;
                sonuc = new List<SelectListItem>();
                sonuc.Add(new SelectListItem
                {
                    Text = "Bir hata oluştu :(",
                    Value = "Default"
                });

            }
            //Oluşturduğum sonucları json olarak geriye gönderiyorum
            return Json(new { ok = basariliMi, text = sonuc });
        }
    }
}