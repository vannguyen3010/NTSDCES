using NTSDCES.Models;
using System.IO;
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

        
        [HttpPost]
        public ActionResult CreateForum([Bind(Include = "PostID,NumReps,Title,NumViews,PostDate,AccountID")] Forum Form, string Username)
        {
            if (ModelState.IsValid)
            {
                // Tạo đối tượng ForumPost thay vì Forum
                var forum = new Forum
                {
                    PostID = db.Fora.Select(f => f.PostID).DefaultIfEmpty(0).Max() + 1,  // Tạo PostID mới
                    NumReps = Form.NumReps,  // Sao chép các thuộc tính từ đối tượng Forum sang ForumPost
                    Title = Form.Title,
                    NumViews = Form.NumViews,
                    PostDate = Form.PostDate,
                    AccountID = db.Accounts.Where(a => a.NameAcc == Username).Select(x => x.AccountID).FirstOrDefault()
                };

                db.Fora.Add(forum);  // Thêm ForumPost vào bảng ForumPosts
                db.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction("Index");  // Chuyển hướng về trang danh sách Forum
            }
            return View(Form.Title);
        }

        public ActionResult DeleteForum(int id)
        {
            Forum ForumPost = db.Fora.FirstOrDefault(x => x.PostID == id);
            db.Fora.Remove(ForumPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}