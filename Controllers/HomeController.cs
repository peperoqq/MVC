using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using prjFinal.Models;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace prjFinal.Controllers
{
    public class HomeController : Controller
    {

        //0000001--------------------------------------------------

        FinalEntities db = new FinalEntities();
        // GET: Home

        int pageSize = 10;
        public ActionResult Index(int page = 1)
        {
            int curren = page < 1 ? 1 : page;
            var product = db.TableFinalExams1091746.OrderByDescending(m => m.運費).ToList();
            var res = product.ToPagedList(curren, pageSize);
            return View(res);
        }



        //02--------------------------------------------------
        public ActionResult Prog2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Prog2(string too)
        {
            var Ad = from m in db.TableFinalExams1091746
                       where m.送貨地址.Contains(too)
                       select m;
            var time = Ad.OrderByDescending(m => m.送貨日期);
            return View("QueryById", time.ToList());
        }
        public ActionResult QueryById()
        {
            return View();
        }


        //00003--------------------------------------------------

        public ActionResult Prog3(string cid="台北市")
        {
            ViewBag.result = db.TableFinalExams1091746
                .Where(m => m.送貨城市 == cid)
                .FirstOrDefault().送貨城市 + "Prog3";
            Class1 vvm = new Class1()
            {
                Table1091746 = db.TableFinalExams1091746.Where(m => m.送貨城市 == cid).ToList()
            };
            return View(vvm);
        }

        public ActionResult Edit(int 訂單號碼)
        {
            var product = db.TableFinalExams1091746
               .Where(m => m.訂單號碼 == 訂單號碼).FirstOrDefault();

            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(TableFinalExams1091746 vproduct)
        {
            if (ModelState.IsValid)
            {
                var temp = db.TableFinalExams1091746
                    .Where(m => m.訂單號碼 == vproduct.訂單號碼).FirstOrDefault();

                temp.客戶編號 = vproduct.客戶編號;
                temp.員工編號 = vproduct.員工編號;
                temp.訂單日期 = vproduct.訂單日期;
                temp.要貨日期 = vproduct.要貨日期;
                temp.送貨日期 = vproduct.送貨日期;
                temp.送貨方式 = vproduct.送貨方式;
                temp.運費 = vproduct.運費;
                temp.收貨人 = vproduct.收貨人;
                temp.送貨地址 = vproduct.送貨地址;
                temp.送貨城市 = vproduct.送貨城市;
                temp.送貨行政區 = vproduct.送貨行政區;
                temp.送貨郵遞區號 = vproduct.送貨郵遞區號;
                temp.送貨國家地區 = vproduct.送貨國家地區;

                db.SaveChanges();
                return RedirectToAction("Prog3");
            }
            return View(vproduct);
        }
        public ActionResult Delete(int 訂單號碼)
        {
            var tty = db.TableFinalExams1091746.Where(m => m.訂單號碼 == 訂單號碼).FirstOrDefault();
            db.TableFinalExams1091746.Remove(tty);
            db.SaveChanges();
            return RedirectToAction("Prog3");
        }


        //04------------------------------------------
        public ActionResult Prog4()
        {

            var kink = new List<string>();
            var kkip = from m in db.TableFinalExams1091746
                         orderby m.送貨城市
                         select m.送貨城市;

            kink.AddRange(kkip.Distinct());

            return View(kink.ToList());
        }
        [HttpPost]
        public ActionResult Prog4(string city)
        {
            var jity = from m in db.TableFinalExams1091746
                        .Where(m => m.送貨城市 == city)
                        select m;

            return View("Showcity", jity.ToList());
        }
        public ActionResult Showcity()
        {
            return View();
        }







        public async Task<ActionResult> Prog5()
        {
            string url =
                "https://data.coa.gov.tw/Service/OpenData/TransService.aspx?UnitId=tR9TIFWlvquB";
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = Int32.MaxValue;

            //connection error handel code
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var response = await httpClient.GetStringAsync(url);
            string path =
                $"{Server.MapPath("JSON").Replace("Home\\", "")}\\ODwsvTransService.json";
            FileInfo fileInfo = new FileInfo(path);
            StreamWriter streamWriter =
                new StreamWriter(path, false, System.Text.Encoding.UTF8);
            streamWriter.WriteLine(response);
            streamWriter.Close();
            return View();
        }



    }
}