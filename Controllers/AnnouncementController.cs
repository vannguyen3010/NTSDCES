using NTSDCES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTSDCES.Controllers
{
    public class AnnouncementController : Controller
    {
        // GET: Announcement
        NTSDCESEntities Data = new NTSDCESEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Progress()
        {
            return View();
        }
        public ActionResult Gamezone()
        {
            return View();
        }
        [Authorize(Roles = "Developer")]
        public ActionResult NewPost()
        {
            return View();
        }
        [Authorize(Roles = "Developer")]
        public ActionResult EditPost(int id)
        {
            ForumPost ForumPost = Data.ForumPosts.FirstOrDefault(x => x.PostID == id);
            return View(ForumPost);
        }
        [Authorize(Roles = "Developer")]
        public ActionResult DeletePost(int id)
        {
            ForumPost ForumPost = Data.ForumPosts.FirstOrDefault(x => x.PostID == id);
            Data.ForumPosts.Remove(ForumPost);
            Data.SaveChanges();
            if (ForumPost.NamePost == "Announcement")
            {
                ForumPost.NamePost = "Index";
            }
            return View(ForumPost.NamePost);
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
        public ActionResult New([Bind(Include = "PostID,NamePost,Content,Images,CatePost,AccountID")] ForumPost Form, HttpPostedFileBase imgfile, string Username)
        {
            if (ModelState.IsValid)
            {
                Form.Images = FileUpLoad(imgfile);
                Form.PostID = Data.ForumPosts.Select(f => f.PostID).DefaultIfEmpty(0).Max() + 1;
                Form.AccountID = Data.Accounts.Where(a => a.NameAcc == Username).Select(x => x.AccountID).FirstOrDefault();
                Form.CatePost = Data.ForumPosts.Where(d => d.NamePost == Form.NamePost).Select(f => f.CatePost).FirstOrDefault();
                Data.ForumPosts.Add(Form);
                Data.SaveChanges();
                if (Form.NamePost == "Announcement")
                {
                    Form.NamePost = "Index";
                }
                return View(Form.NamePost);
            }
            return View(Form.NamePost);
        }
        public ActionResult Edit(ForumPost Form, HttpPostedFileBase imgfile, string Username)
        {
            Form.Images = FileUpLoad(imgfile);
            Form.CatePost = Data.ForumPosts.Where(d => d.NamePost == Form.NamePost).Select(f => f.CatePost).FirstOrDefault();
            Form.AccountID = Data.Accounts.Where(a => a.NameAcc == Username).Select(x => x.AccountID).FirstOrDefault();
            Data.Entry(Form).State = EntityState.Modified;
            Data.SaveChanges();
            if (Form.NamePost == "Announcement")
            {
                Form.NamePost = "Index";
            }
            return View(Form.NamePost);
        }
    }
}