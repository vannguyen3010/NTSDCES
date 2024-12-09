using NTSDCES.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace NTSDCES.Controllers
{
    public class AccountController : Controller
    {
        // GET: LogIn
        NTSDCESEntities Data = new NTSDCESEntities();
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult RecoverAccount()
        {
            return View();
        }
        public static string GetGM5(string str)
        {
            MD5 mD5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = mD5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i = i + 1)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
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
        public ActionResult LogIn(string Username, string Passwords)
        {
            bool IsValidUser = Data.Accounts.Any(u => u.NameAcc == Username && u.Passwords == Passwords);
            if (IsValidUser == true)
            {
                int CateAcc = Data.Accounts.Where(s => s.NameAcc == Username).Select(d => d.CateAcc).FirstOrDefault();
                if (CateAcc == 1 || CateAcc == 0)
                {
                    FormsAuthentication.SetAuthCookie(Username, false);
                    return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                }
                else
                {
                    if (CateAcc == 2)
                    {
                        FormsAuthentication.SetAuthCookie(Username, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View();
                    }
                }    
            }
            else
            {
                ModelState.AddModelError("", "invalid Username or Password");
                return View();
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Create(Account Acc, HttpPostedFileBase Avatar)
        {
            if (ModelState.IsValid)
            {
                Acc.AccountID = Data.Accounts.Select(f => f.AccountID).DefaultIfEmpty(0).Max() + 1;
                Acc.Avatar = FileUpLoad(Avatar);
                Acc.Roles = "Guest";
                Acc.CateAcc = 2;
                Data.Accounts.Add(Acc);
                Data.SaveChanges();
                return RedirectToAction("LogIn", "Account");
            }
            return View("LogIn");
        }
    }
}