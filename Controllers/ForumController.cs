using NTSDCES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTSDCES.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        NTSDCESEntities db = new NTSDCESEntities();
        public ActionResult Index()
        {
            var forums = db.Fora.ToList();
            return View(forums);
        }

        public ActionResult Forum_post1()
        {
            return View();
        }

        //Action xử lý khi click vào Title, tăng RepliesCount
        [HttpPost]
        public ActionResult IncrementRepliesCount(int id)
        {
            using (var db = new NTSDCESEntities())
            {
                var forum = db.Fora.SingleOrDefault(f => f.PostID == id);
                if (forum != null)
                {
                    // Tăng giá trị NumReps
                    forum.NumReps = (forum.NumReps ?? 0) + 1;
                    db.SaveChanges(); // lưu vào database
                }
            }
            return RedirectToAction("Index");
        }
    }
}