using NTSDCES.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTSDCES.Controllers
{
    public class SupportController : Controller
    {
        // GET: Support
        NTSDCESEntities Data = new NTSDCESEntities();
        public ActionResult Index()
        {
            return View();
        }
        public string FileUpLoad(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = Server.MapPath("~/images/");
                string filename = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(path, filename);
                file.SaveAs(fullPath);
                return filename;
            }
            else 
            {
                return null;
            }
        }
        [HttpPost]
        [Authorize(Roles = "Guest")]
        public ActionResult Index([Bind(Include = "FormID,Username,Email,Title,Discribe,Images,AccountID")] SupportForm Form, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                Form.Images = FileUpLoad(imgfile);
                Form.FormID = Data.SupportForms.Select(f => f.FormID).DefaultIfEmpty(0).Max() + 1;
                Form.AccountID = Data.Accounts.Where(a => a.NameAcc == Form.Username).Select(x => x.AccountID).FirstOrDefault();
                Data.SupportForms.Add(Form);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Form");
        }
    }
}