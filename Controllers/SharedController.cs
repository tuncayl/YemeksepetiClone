using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tamircisepeti22.Models;
using System.Web.Security;

namespace Tamircisepeti22.Controllers
{
   
    public class SharedController : Controller
    {

        TmrsepetiEntities db = new TmrsepetiEntities();
        Adre modela = new Adre();
        Seller seller = new Seller();   
        siparisler siparis= new siparisler();

        // GET: Shared
        public ActionResult _Layout()
        {
            return View();
        }
        [HttpPost]
        public JsonResult adreskayit(Adre adres)
        {
            var cookie = Request.Cookies["Kullancibilgi"];
            int uid = Convert.ToInt32(cookie["id"].ToString());
            var degistokus=db.Adres.FirstOrDefault(x=>x.auid==uid);
            if (degistokus == null)
            {
                
                modela.acity = 20;
                modela.auid = uid;
                modela.asellerid = null;
                modela.atown = adres.atown;
                modela.adistrict = adres.adistrict;
                modela.aadres = adres.aadres;
                db.Adres.Add(modela);
                db.SaveChanges();
            }
            else
            {
                degistokus.acity= 20;
                degistokus.auid= uid;
                degistokus.asellerid = null;
                degistokus.atown = adres.atown;
                degistokus.adistrict=adres.adistrict;
                degistokus.aadres= adres.aadres;

                db.SaveChanges();
            }


           

            return Json("Kayıt eklenmiştir");

        }
        [HttpPost]
        public JsonResult kelimekaydet(Seller seller)
        {
            var cookie = Request.Cookies["Kullancibilgi"];
            cookie["kelime"]=seller.Rname;
            
            Response.Cookies.Add(cookie);

            return Json("okey");

        }
        [HttpPost]
        public JsonResult adresev(int? bos)
        {
           
            List<SelectListItem> sonuc = new List<SelectListItem>();
            var cookie = Request.Cookies["Kullancibilgi"];
            bool basariliMi = true;
            int uid = Convert.ToInt32(cookie["id"].ToString());

            try
            {
                foreach (var adres in db.Adres.Where(id => id.auid == uid).ToList())
                {
                    foreach (var adres1 in db.Ilcelers.Where(ilceid => ilceid.IlceID == adres.atown).ToList())
                    {

                        foreach (var adres2 in db.tCity_District_Street_Town.Where(semtid => semtid.id == adres.adistrict).ToList())
                        {
                            
                            sonuc.Add(new SelectListItem
                            {
                                Text = adres1.Ilce + " ( " + adres2.mahalle + " )".ToString()
                           
                               
                        });

                        }

                    }
                }
            }
            
               catch (Exception)
            {
                //hata ile karşılaşırsak buraya düşüyor
                basariliMi = false;
                sonuc = new List<SelectListItem>();
                sonuc.Add(new SelectListItem
                {
                    Text = "Adres yok "
                   
                });

            }

            
            return Json(new { ok = basariliMi, text = sonuc });
        }
        [HttpPost]
        public JsonResult adresgetirgötür(int? ilID, string tip)
        {
            var cookie = Request.Cookies["Kullancibilgi"];
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
                    case "adresgetir":
                       
                        
                        int uid = Convert.ToInt32(cookie["id"].ToString());

                        try
                        {
                            foreach (var adres in db.Adres.Where(id => id.auid == uid).ToList())
                            {
                                foreach (var adres1 in db.Ilcelers.Where(ilceid => ilceid.IlceID == adres.atown).ToList())
                                {

                                    foreach (var adres2 in db.tCity_District_Street_Town.Where(semtid => semtid.id == adres.adistrict).ToList())
                                    {
                                        cookie["adres"] = adres1.Ilce + " ( " + adres2.mahalle + " )".ToString();
                                        Response.Cookies.Add(cookie);
                                        sonuc.Add(new SelectListItem
                                        {
                                            Text = adres1.Ilce + " ( " + adres2.mahalle + " )".ToString()

                                        });

                                    }

                                }
                            }
                        }

                        catch (Exception)
                        {
                            //hata ile karşılaşırsak buraya düşüyor
                            basariliMi = false;
                            sonuc = new List<SelectListItem>();
                            sonuc.Add(new SelectListItem
                            {
                                Text = "Adres yok "

                            });

                        }


                        return Json(new { ok = basariliMi, text = sonuc });

                     
                    case "ilceGetir":
                        //ilcelerimizi getireceğiz ilimizi selecten seçilen ilID sine göre 
                        foreach (var ilce in db.Ilcelers.Where(il => il.IlID == 20).ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = ilce.Ilce,
                                Value = ilce.IlceID.ToString()
                            });
                        }
                        break;
                    case "semtgetir":

                        foreach (var ilceisim in db.Ilcelers.Where(isim => isim.IlceID == ilID).ToList())
                        {

                            foreach (var semt in db.tCity_District_Street_Town.Where(mah => mah.ilce == ilceisim.Ilce.ToUpper().ToString()).ToList()) //yordum biraz
                            {

                                sonuc.Add(new SelectListItem
                                {
                                    Text = semt.mahalle,
                                    Value = semt.id.ToString()
                                });

                            }

                        }
                        break;

                    case "sepetsil":
                        int userid2 = Convert.ToInt32(cookie["id"].ToString());
                        sonuc.Clear();
                        foreach (var sepet in db.sepets.Where(userd => userd.userid == userid2 && !userd.kullanıldı).ToList())
                        {
                            db.sepets.Remove(sepet);

                        }
                        db.SaveChanges();
                        basariliMi = true;
                        return Json(new { ok = basariliMi });
                    case "siparisver":
                        int userid5 = Convert.ToInt32(cookie["id"].ToString());
                        int sellerid5 = Convert.ToInt32(cookie["sellerid"].ToString());
                        var sipariskontrol = db.siparislers.FirstOrDefault(x => x.userid == userid5 && (!x.onaylandı && !x.yolda)); //buraya dikkat !!!!
                        if (sipariskontrol == null)
                        {
                            foreach (var sepet in db.sepets.Where(userd => userd.userid == userid5).ToList())
                            {

                                siparis.userid = userid5;
                                siparis.sellerid = sellerid5;
                                siparis.sepetid = sepet.sepetid;
                                siparis.yolda = false;
                                siparis.onaylandı = false;
                                db.siparislers.Add(siparis);
                                sepet.kullanıldı = true;

                            }
                            db.SaveChanges();
                            basariliMi = true;
                            return Json(new { ok = basariliMi, text = sonuc });
                        }
                        else
                        {
                            basariliMi = false;
                            return Json(new { ok = basariliMi, text = sonuc });
                        }
                    case "sipariskontrol":
                        int userid6 = Convert.ToInt32(cookie["id"].ToString());
                        bool yoldami=false;
                        bool onay = false;
                        var sipariskontrol2 = db.siparislers.FirstOrDefault(x => x.userid == userid6 && (!x.yolda || !x.onaylandı)); //buraya dikkat !!!!
                        if (sipariskontrol2 != null)
                        {
                            
                            foreach (var siparis in db.siparislers.Where(userd => userd.userid == userid6).ToList())
                            {

                                foreach (var dükkan in db.Sellers.Where(userd => userd.Rsellerid == siparis.sellerid).ToList())
                                {
                                    sonuc.Add(new SelectListItem
                                    {
                                        Text = dükkan.Rname.ToString(),
                                        Value = dükkan.Rtel.ToString(),
                                        Selected = sipariskontrol2.yolda,
                                        Disabled=sipariskontrol2.onaylandı
                                        
                                    }) ;

                                }

                            }
                            if(sonuc[0].Selected) yoldami= true;
                            if (sonuc[0].Disabled && yoldami)
                            {
                               
                                onay = true;
                                db.siparislers.Remove(sipariskontrol2);
                            }
                            if(sonuc.Count > 0)
                            {
                                basariliMi = true;
                                return Json(new { ok = basariliMi, text = sonuc, yazi = yoldami,yazi1=onay});
                            }
                        }
                            basariliMi = false;
                            return Json(new { ok = basariliMi, text = sonuc });

                        



                    case "sepetcontrol":
                        int userid3 = Convert.ToInt32(cookie["id"].ToString());
                        sonuc.Clear();
                        foreach (var sepet in db.sepets.Where(userd => userd.userid == userid3 && !userd.kullanıldı).ToList())
                        {

                            foreach (var ad in db.hizmetlers.Where(hizme => hizme.hizmetid == sepet.hizmetid).ToList())
                            {

                                sonuc.Add(new SelectListItem
                                {
                                    Text = ad.hizmetadi.ToString(),
                                    Value = ad.hizmetfiyati.ToString()



                                });


                            }

                        }
                        if (sonuc.Count > 0)
                        {
                            basariliMi = true;

                        }
                        else basariliMi = false;
                        return Json(new { ok = basariliMi, text = sonuc });
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