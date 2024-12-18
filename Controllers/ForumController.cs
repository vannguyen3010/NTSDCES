using NTSDCES.Models;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NTSDCES.Controllerss
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
        public ActionResult UpdateStatus()
        {
            var forums = db.Fora.ToList();
            return View(forums);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatusBrowse(int id, bool? status)
        {
            // Lấy bài viết cần cập nhật trạng thái
            var forum = db.Fora.FirstOrDefault(x => x.PostID == id);
            if (forum == null)
            {
                return HttpNotFound(); // Trả về 404 nếu không tìm thấy bài viết
            }

            // Cập nhật trạng thái
            forum.Status = status ?? false;

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Trả về trang danh sách hoặc giữ nguyên trang hiện tại
            return RedirectToAction("UpdateStatus"); // Hoặc trả về trang hiện tại
        }




        public ActionResult ForumEdit(int id)
        {
            Forum ForumPost = db.Fora.FirstOrDefault(x => x.PostID == id);
            return View(ForumPost);
        }

        public ActionResult ForumDetail(int id)
        {
            //var forum = db.Fora.FirstOrDefault(x => x.PostID == id);
            var forum = db.Fora.Include(f => f.Account).FirstOrDefault(x => x.PostID == id);

            if (forum == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy Forum với ID
            }
            return View(forum);

        }
        

        //Action xử lý khi click Like, tăng Replies
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
                    db.SaveChanges(); //Sao đó lưu dô database
                }
            }
            return RedirectToAction("Index");
        }

        //Action xử lý khi click vào Dislike, giảm Replies
        [HttpPost]
        public ActionResult DislikeRepliesCount(int id)
        {
            using (var db = new NTSDCESEntities())
            {
                var forum = db.Fora.SingleOrDefault(f => f.PostID == id);
                if (forum != null)
                {
                    // Tăng giá trị NumReps
                    forum.NumReps = (forum.NumReps ?? 0) - 1;
                    db.SaveChanges(); //Sao đó lưu dô database
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateForum([Bind(Include = "PostID,NumReps,Title,NumViews,PostDate, AccountID, Description,Images")] Forum Form, HttpPostedFileBase imgfile, string Username)
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
                    Status = false,
                    AccountID = db.Accounts.Where(a => a.NameAcc == Username).Select(x => x.AccountID).FirstOrDefault(),
                    Description = Form.Description,
                    Images = FileUpLoad(imgfile)
                };

                db.Fora.Add(forum);  // Thêm Data vào bảng Forum
                db.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction("Index");  // Chuyển hướng về trang danh sách Forum
            }
            return View(Form.Title);
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

        public ActionResult DeleteForum(int id) //phần này phần delete ko có j đâu
        {
            Forum ForumPost = db.Fora.FirstOrDefault(x => x.PostID == id);
            db.Fora.Remove(ForumPost);   
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditForumPost(int id, [Bind(Include = "PostID, Title, NumViews, PostDate, Description, Images")] Forum forum, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                // Lấy bài viết cần chỉnh sửa
                var existingForum = db.Fora.FirstOrDefault(x => x.PostID == id);
                if (existingForum == null)
                {
                    return HttpNotFound(); // Nếu không tìm thấy bài viết, trả về lỗi 404
                }

                // Cập nhật thông tin từ form vào bài viết hiện tại
                existingForum.Title = forum.Title;
                existingForum.NumViews = forum.NumViews;
                existingForum.PostDate = forum.PostDate;
                existingForum.Description = forum.Description;

                // Kiểm tra nếu người dùng chọn ảnh mới
                if (imgfile != null)
                {
                    existingForum.Images = FileUpLoad(imgfile); // Lưu hình ảnh mới
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return RedirectToAction("ForumDetail", new { id = existingForum.PostID }); // Chuyển hướng đến trang chi tiết bài viết
            }

            return View(forum); // Nếu dữ liệu không hợp lệ, hiển thị lại form chỉnh sửa
        }



    }
}