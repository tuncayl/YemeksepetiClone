using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tamircisepeti22.Models;

namespace tamircisepeti22.Controllers
{
    [AllowAnonymous]
    public class cantaController : Controller
    {
        TmrsepetiEntities db = new TmrsepetiEntities();
        Seller model = new Seller();
    
        Adre amodel=new Adre();
        // GET: canta
        public ActionResult giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult giris(Seller seller)
        {
            var userIndb = db.Sellers.FirstOrDefault(x => x.Rkulad == seller.Rkulad && x.Rsifre == seller.Rsifre);
            if (userIndb != null && userIndb.Rauthid==1)
            {
                FormsAuthentication.SignOut();
                Session["control"] =userIndb.Rsellerid.ToString();
                HttpCookie cookie = new HttpCookie("saticibilgi");
                cookie["ad"] = userIndb.Rname;
                cookie["tel"] = userIndb.Rtel;
                cookie["email"] = userIndb.Remail;
                Response.Cookies.Add(cookie);
                return RedirectToAction("home", "cantahome");

            }
            else
            {

                return View();
            }
        }
        [HttpPost]
        public JsonResult IlIlce(int? ilID, string tip)
        {
            //EntityFramework ile veritabanı modelimizi oluşturduk ve
            //IlilceDBEntities ile db nesnesi oluşturduk.
            TmrsepetiEntities db = new TmrsepetiEntities();
            //geriye döndüreceğim sonucListim
            List<SelectListItem> sonuc = new List<SelectListItem>();
            //bu işlem başarılı bir şekilde gerçekleşti mi onun kontrolunnü yapıyorum
            bool basariliMi = true;
            try
            {
                switch (tip)
                {
                    case "ilGetir":
                        //veritabanımızdaki iller tablomuzdan illerimizi sonuc değişkenimze atıyoruz
                        foreach (var il in db.Illers.ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = il.Il,
                                Value = il.IlID.ToString()
                            });
                        }
                        break;
                    case "ilceGetir":
                        //ilcelerimizi getireceğiz ilimizi selecten seçilen ilID sine göre 
                        foreach (var ilce in db.Ilcelers.Where(il => il.IlID == ilID).ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = ilce.Ilce,
                                Value = ilce.IlceID.ToString()
                            });
                        }
                        break;
                    case "ssemtgetir":

                        foreach (var ilceisim in db.Ilcelers.Where(isim => isim.IlceID == ilID).ToList())
                        {

                            foreach (var semt in db.tCity_District_Street_Town.Where(mah =>mah.ilce==ilceisim.Ilce.ToUpper().ToString()).ToList()) //yordum biraz
                            {

                                sonuc.Add(new SelectListItem
                                {
                                    Text = semt.mahalle,
                                    Value = semt.id.ToString()
                                });

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
        public ActionResult cikisyap()
        {

            Session.Remove("control");
            HttpContext.Response.Cookies["saticibilgi"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("giris");
        }
        public ActionResult kayitol()
        {
            return View();
        }
        [HttpPost]
        public ActionResult kayitol(Seller seller,Adre adres)
        {

               
                var userIndb = db.Sellers.FirstOrDefault(x => x.Rkulad == seller.Rkulad || x.Remail == seller.Remail);
                if (userIndb == null)
                {
                    model.Remail = seller.Remail;
                    model.Rsifre = seller.Rsifre;
                    model.Rname = seller.Rname;
                    model.Rkulad = seller.Rkulad;
                    model.Rtel = seller.Rtel;
                    model.Rauthid = 1;
                    db.Sellers.Add(model);
                    amodel.aadres = adres.aadres;
                    amodel.acity = adres.acity;
                    amodel.atown = adres.atown;
                    amodel.adistrict = adres.adistrict;
                    amodel.asellerid = model.Rsellerid;
                  
                 
                   db.Adres.Add(amodel);
                    db.SaveChanges();
                    Session["control"] = "!sşal2i4lşsopaş1";
                   
                return RedirectToAction("giris", "canta");

                }
                else
                {

                    return RedirectToAction("kayitol");
                }
            
         

        }
    }
}