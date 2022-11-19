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
    public class cantahomeController : Controller
    {
        TmrsepetiEntities db = new TmrsepetiEntities();
        hizmetler modelh=new hizmetler();   
        // GET: cantahome
        [HttpPost]
        public JsonResult hizmetimjs2(hizmetler hizmet)
        {
            
                modelh.hizmetadi = hizmet.hizmetadi;
                modelh.hizmetfiyati=hizmet.hizmetfiyati;
                modelh.sellerid=Convert.ToInt32(Session["control"].ToString());
                db.hizmetlers.Add(modelh);
                db.SaveChanges();


            return Json("Kayıt eklenmiştir");


        }

        [HttpPost]
        public JsonResult hizmetimjs(int? bos)
        {
 
            int sellerid=Convert.ToInt32(Session["control"].ToString());
            List <SelectListItem> sonuc = new List<SelectListItem>();
            bool basariliMi = true;
            foreach (var hizmetler in db.hizmetlers.Where(id=>id.sellerid==sellerid).ToList())
            {
                sonuc.Add(new SelectListItem
                {
                    Text = hizmetler.hizmetadi,
                    Value=hizmetler.hizmetfiyati.ToString(),
                    
                });
            }


            return Json(new { ok = basariliMi, text = sonuc });

        }
        [HttpPost]
        public JsonResult sipariskontrol(int? ID, string tip)
        {
            List<SelectListItem> sonuc = new List<SelectListItem>();
            bool basariliMi = true;
            int sellerid = Convert.ToInt32(Session["control"].ToString());
            bool yoldami = false;
            bool onay = false;

            switch (tip)
            {
                case "sipkontrol":
                    var sipariskontrol2 = db.siparislers.FirstOrDefault(x => x.sellerid == sellerid && (!x.onaylandı || !x.yolda)); //buraya dikkat !!!!
                    if (sipariskontrol2 != null)
                    {
                        foreach (var siparis1 in db.siparislers.Where(userd => userd.sellerid == sellerid).ToList())
                        {

                            foreach (var kullancibilgi in db.users.Where(userd => userd.Uid == siparis1.userid).ToList())
                            {
                                foreach (var adres in db.Adres.Where(aadres => aadres.auid == kullancibilgi.Uid).ToList())
                                {
                                    foreach (var adresmah in db.tCity_District_Street_Town.Where(aaadres => aaadres.id == adres.adistrict).ToList())
                                    {
                                        sonuc.Add(new SelectListItem
                                        {
                                            Text = kullancibilgi.Uname + " " + kullancibilgi.USurname + "  ( " + adresmah.mahalle + " ) " + adres.aadres,
                                            Value = kullancibilgi.Uid.ToString(),
                                            Selected = sipariskontrol2.yolda,
                                            Disabled = sipariskontrol2.onaylandı,


                                        });
                                    }
                                }
                            }

                        }
                        if (sonuc[0].Selected) yoldami = true;
                        if (sonuc[0].Disabled && yoldami) onay = true;
                        if (sonuc.Count > 0)
                        {
                            basariliMi = true;
                            return Json(new { ok = basariliMi, text = sonuc, yazi = yoldami, yazi1 = onay });
                        }
                    }
                    basariliMi = false;
                    return Json(new { ok = basariliMi, text = sonuc });
                case "sipyol":
                    foreach (var siparis2 in db.siparislers.Where(userd => userd.userid == ID).ToList())
                    {
                        siparis2.yolda = true;
                    }
                    db.SaveChanges();
                    basariliMi = true;
                    return Json(new {ok=basariliMi,text=sonuc}); ;
                case "siponay":
                    foreach (var siparis2 in db.siparislers.Where(userd => userd.userid == ID).ToList())
                    {
                        siparis2.onaylandı = true;
                        
                    }
                    db.SaveChanges();
                    var sip4 = db.siparislers.FirstOrDefault(x => x.sellerid == sellerid && x.onaylandı);
                    db.siparislers.Remove(sip4);
                    db.SaveChanges();
                    basariliMi = true;
                    return Json(new { ok = basariliMi, text = sonuc }); ;
                default: return Json(null);

            }

        }
        public ActionResult home()
        {
            if (Session["control"] == null)
            {
                return RedirectToAction("giris", "canta");
            }
            else
            {
                return View();
            }
           
        }
        public ActionResult siparis()
        {
            if (Session["control"] == null)
            {
                return RedirectToAction("giris", "canta");
            }
            else
            {
                return View();
            }

        }
        public ActionResult hizmetim()
        {
            if (Session["control"] == null)
            {
                return RedirectToAction("giris", "canta");
            }
            else
            {
                return View();
            }

        }
        public ActionResult saataraligi()
        {
            if (Session["control"] == null)
            {
                return RedirectToAction("giris", "canta");
            }
            else
            {
                return View();
            }

        }

    }
}