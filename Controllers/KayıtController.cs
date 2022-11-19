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
    public class KayıtController : Controller
    {
        TmrsepetiEntities db = new TmrsepetiEntities(); 
        user model = new user();
        Adre adres = new Adre();

        // GET: Kayıt
        [Route("Kayıt")]
        public ActionResult _Kayıt()
        {
            return View();
        }
     
        [HttpPost]
  
        public ActionResult _Kayıt(user user)
        {
            try
            {
                var userIndb = db.users.FirstOrDefault(x => x.UKullaniciad== user.UKullaniciad || x.Uemail == user.Uemail);
                if (userIndb == null)
                {
                    
                    model.Uemail = user.Uemail;
                    model.Upassword = user.Upassword;
                    model.Uname = user.Uname;
                    model.USurname = user.USurname;
                    model.UKullaniciad= user.UKullaniciad;
                    model.Utarih = user.Utarih;
                    model.Uauthid = 0;
                    db.users.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                   
                }
                else
                {

                    return RedirectToAction("_kayıt");
                }
            }
            catch
            {
                return RedirectToAction("_kayıt");
            }
        
           

        
        }
        
    }
}