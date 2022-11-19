using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tamircisepeti22.Models;
using System.Web.Security;
namespace tamircisepeti22.Controllers
{
  
    public class hesapController : Controller
    {
        
        
        TmrsepetiEntities db = new TmrsepetiEntities();
        Seller Seller=new Seller(); 
        Adre   adresm=new Adre();
        sepet sepet=new sepet();    
        siparisler siparis=new siparisler();
       [HttpPost]
       public JsonResult tamircigetir(int? ID,string tip)
        {
           
            var cookie = Request.Cookies["Kullancibilgi"];
            List<SelectListItem> sonuc = new List<SelectListItem>();
            bool basariliMi = true;
            switch (tip)
            {
                case "tamircigetir":
                    
                    int userid = Convert.ToInt32(cookie["id"].ToString());
                   
                    
                    foreach (var sel in db.Adres.Where(id => id.auid == userid).ToList())
                    {
                        foreach (var adres in db.Adres.Where(id => id.atown == sel.atown && id.auid == null).ToList())
                        {
                            foreach (var seller in db.Sellers.Where(id => id.Rsellerid == adres.asellerid).ToList())
                            {
                                sonuc.Add(new SelectListItem
                                {
                                    Text = (seller.Rname + "                       " + seller.Rtel).ToString(),
                                    Value=seller.Rsellerid.ToString(),
                                });;
                            }
                        }
                    }
                    return Json(new { ok = basariliMi, text = sonuc });
                case "tamircigetirara":

                    int userid2 = Convert.ToInt32(cookie["id"].ToString());
                    //string kelime=cookie["aranan"]
                    string kelime=cookie["kelime"].ToString();
                   
                       
                            foreach (var seller in db.Sellers.Where(ad=>ad.Rname.Contains(kelime)))
                            {
                                sonuc.Add(new SelectListItem
                                {
                                    Text = (seller.Rname + "                       " + seller.Rtel).ToString(),
                                    Value = seller.Rsellerid.ToString(),
                                }); ;
                            }
                        
                    
                    return Json(new { ok = basariliMi, text = sonuc });


                case "tamircigötür":
                    
                    int rsellerid = Convert.ToInt32(ID);
                    cookie["saticiid"] = rsellerid.ToString();
                    int sellerid = Convert.ToInt32(cookie["saticiid"]);
                    foreach (var adres in db.Adres.Where(id => id.asellerid == sellerid).ToList())
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
                    var userIndb = db.Sellers.FirstOrDefault(x => x.Rsellerid == sellerid);
                    if (userIndb != null)
                    {
                        string adres = sonuc[0].Text.ToString();
                        cookie["saticiadi"] = userIndb.Rname;
                        cookie["saticitel"] = userIndb.Rtel;
                        cookie["saticiadres"] = adres;
                        Response.Cookies.Add(cookie);
                    }
                   
                    return Json(new {ok=basariliMi});
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
                case "sepetsil":
                    int userid0 = Convert.ToInt32(cookie["id"].ToString());
                    sonuc.Clear();
                    foreach (var sepet in db.sepets.Where(userd => userd.userid == userid0 && !userd.kullanıldı).ToList())
                    {
                        db.sepets.Remove(sepet);

                    }
                    db.SaveChanges();
                    basariliMi = true;

                    return Json(new { ok = basariliMi });
                case "sepetcontrol":
                    int userid3 = Convert.ToInt32(cookie["id"].ToString());
                    sonuc.Clear();
                    foreach (var sepet in db.sepets.Where(userd => userd.userid == userid3 && !userd.kullanıldı).ToList())
                    {

                        foreach (var ad in db.hizmetlers.Where(hizme => hizme.hizmetid == sepet.hizmetid ).ToList())
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
                     return Json(null);
                                 

            }
            

        }
        [HttpPost]
        public JsonResult tamircibilgi(int?ID,string tip)
        {

            bool basariliMi = true;
            List<SelectListItem> sonuc = new List<SelectListItem>();
            var cookie = Request.Cookies["Kullancibilgi"];
            int sellerid = Convert.ToInt32(cookie["saticiid"]);
            sonuc.Clear();
            switch (tip)
                {

                    case "tamircibilgim":
                    
                    foreach (var adres in db.Adres.Where(id => id.asellerid == sellerid).ToList())
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
                        var userIndb = db.Sellers.FirstOrDefault(x => x.Rsellerid == sellerid);
                        if (userIndb != null)
                        {
                            string adres = sonuc[0].Text.ToString();
                            cookie["saticiadi"] = userIndb.Rname;
                            cookie["saticitel"] = userIndb.Rtel;
                            cookie["saticiadres"] = adres;
                            Response.Cookies.Add(cookie);
                        }
                    return Json(null);

                case "tamircihizmetim":
                    
                    foreach (var hizmet in db.hizmetlers.Where(id => id.sellerid == sellerid).ToList())
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = hizmet.hizmetadi + "  " + hizmet.hizmetfiyati + "  ".ToString(),
                            Value = hizmet.hizmetid.ToString(),


                        });




                    }
                    return Json(new { ok = basariliMi, text = sonuc });
                case "tamircisiparis":
                    
                    foreach (var hizmet in db.hizmetlers.Where(id => id.hizmetid == ID).ToList())
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = hizmet.hizmetadi + "  " + hizmet.hizmetfiyati + "  ".ToString(),
                            Value=hizmet.sellerid.ToString()
                            
                           
                        });
                       

                    }
                    cookie["hizmetid"] =ID.Value.ToString();
                    cookie["sellerid"] = sonuc[0].Value.ToString();
                    Response.Cookies.Add(cookie);
                    return Json(new {ok=basariliMi,text=sonuc }) ;
                case "sepetekle":
                   
                    int userid4= Convert.ToInt32(cookie["id"].ToString());
                    int hizmetid= Convert.ToInt32(cookie["hizmetid"].ToString());

                    var sepetkontrol = db.sepets.FirstOrDefault(x => x.userid == userid4 &&!x.kullanıldı);
                    if(sepetkontrol == null)
                    {

                        sepet.userid = userid4;
                        sepet.hizmetid = hizmetid;
                        db.sepets.Add(sepet);
                        db.SaveChanges();
            
                        return Json(new { ok = basariliMi, text = sonuc });
                    }
                    else
                    {
                        int userid3 = Convert.ToInt32(cookie["id"].ToString());
                        int sepetsellerid = Convert.ToInt32(cookie["sellerid"].ToString());
                        int hizmetid3 = Convert.ToInt32(cookie["hizmetid"].ToString());
                        foreach (var sepet in db.sepets.Where(userd =>userd.userid==userid3 &&!userd.kullanıldı).ToList())
                        {

                        foreach (var ad in db.hizmetlers.Where(hizme => hizme.hizmetid ==sepet.hizmetid ).ToList())
                         {

                            sonuc.Add(new SelectListItem
                            {
                                Text=ad.hizmetid.ToString(),
                                Value = ad.sellerid.ToString()


                            }) ; 
                         }
                        }
                        var eskihizmet = sonuc.FirstOrDefault(x => x.Text == hizmetid3.ToString());
                       if (Convert.ToInt32(sonuc[0].Value.ToString())== sepetsellerid && eskihizmet==null)
                        {
                            sepet.userid = Convert.ToInt32(cookie["id"].ToString());
                            sepet.hizmetid = Convert.ToInt32(cookie["hizmetid"].ToString());
                            db.sepets.Add(sepet);
                            db.SaveChanges();
                            cookie["sellerid"] = sonuc[0].Value.ToString();
                            cookie["hizmetid"] = sonuc[0].Text.ToString();
                            Response.Cookies.Add(cookie);
                            return Json(new { ok = basariliMi, text = sonuc });
                        }
                        else
                        {
                            basariliMi = false;
                            return Json(new { ok = basariliMi});
                        }
                    }
                case "siparisver":
                    int userid5 = Convert.ToInt32(cookie["id"].ToString());
                    int sellerid5 = Convert.ToInt32(cookie["sellerid"].ToString());
                    var sipariskontrol = db.siparislers.FirstOrDefault(x => x.userid == userid5 && ( !x.onaylandı && !x.yolda)); //buraya dikkat !!!!
                    if(sipariskontrol == null) 
                    {
                        foreach (var sepet in db.sepets.Where(userd => userd.userid == userid5).ToList())
                        {

                            siparis.userid = userid5;
                            siparis.sellerid=sellerid5;
                            siparis.sepetid = sepet.sepetid;
                            siparis.yolda = false;
                            siparis.onaylandı = false;
                            db.siparislers.Add(siparis);
                            sepet.kullanıldı = true;

                        }
                        db.SaveChanges();
                        basariliMi=true;    
                        return Json(new { ok = basariliMi, text = sonuc });
                    }
                    else
                    {
                        basariliMi = false;
                        return Json(new { ok = basariliMi, text = sonuc });
                    }

                case "sepetcontrol":
                    int userid = Convert.ToInt32(cookie["id"].ToString());
                    sonuc.Clear();
                    foreach (var sepet in db.sepets.Where(userd =>userd.userid==userid &!userd.kullanıldı).ToList())
                    {

                        foreach (var ad in db.hizmetlers.Where(hizme => hizme.hizmetid == sepet.hizmetid).ToList())
                        {

                            sonuc.Add(new SelectListItem
                            {
                                Text = ad.hizmetadi.ToString(),
                        
                                Value = ad.hizmetfiyati.ToString()    

                            }) ; 


                        }

                    }
                    if (sonuc.Count > 0)
                    {
                        basariliMi = true;
                        
                    }
                    else basariliMi = false;
                    return Json(new { ok = basariliMi, text = sonuc });

                case "sepetsil":
                    int userid2= Convert.ToInt32(cookie["id"].ToString());
                    sonuc.Clear();
                    foreach (var sepet in db.sepets.Where(userd => userd.userid == userid2 & !userd.kullanıldı).ToList())
                    {
                         db.sepets.Remove(sepet);

                    }
                    db.SaveChanges();
                    basariliMi = true;
                    basariliMi = false;
                    return Json(new { ok = basariliMi});

                default: 
                    return Json(null);

            }
            
            

           
           
        }
        [HttpPost]
        public JsonResult tamircihizmet(string tip)
        {
            bool basariliMi = true;
            List<SelectListItem> sonuc = new List<SelectListItem>();
            var cookie = Request.Cookies["Kullancibilgi"];
            int sellerid = Convert.ToInt32(cookie["saticiid"]);
            switch (tip)
            {
                case "tamircihizmet":

                    foreach (var hizmet in db.hizmetlers.Where(id => id.sellerid == sellerid).ToList())
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = hizmet.hizmetadi + "  " + hizmet.hizmetfiyati + "  ".ToString(),
                            Value = hizmet.hizmetid.ToString()

                        });




                    }
                    return Json(new { sonuc, ok = basariliMi });
                default:
                    return Json(null);
            }
        }
        public ActionResult hizmetgör()
        {
            return View();
        }
        public ActionResult tamirciara()
        {
            return View();
        }
        public ActionResult siparisal()
        {
            return View();
        }
        public ActionResult profilim()
        {
            return View();
        }
        public ActionResult siparişlerim()
        {
            return View();
        }
        public ActionResult favorilerim()
        {
            return View();
        }
        public ActionResult adreslerim()
        {
            return View();
        }
        public ActionResult bilgilerim()
        {
            return View();
        }
    }
}