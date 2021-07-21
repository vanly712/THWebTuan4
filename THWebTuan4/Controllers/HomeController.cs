using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THWebTuan4.Models;

namespace THWebTuan4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBConnect context = new DBConnect();
            var upcoming = context.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            var userId = User.Identity.GetUserId();
            foreach (Course course in upcoming)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(course.LecturerId);
                course.LecturerId = user.Name;
                course.Name = user.Name;

                if (userId != null)
                {
                    course.isLogin = true;
                    Attendance find = context.Attendances.FirstOrDefault(p => p.CourseID == course.Id && p.Attendee == userId);
                    if (find == null)
                        course.isShowGoing = true;
                    Following findFollow = context.Followings.FirstOrDefault(p => p.FollowerId == userId && p.FolloweeId == course.LecturerId);
                    if (findFollow == null)
                        course.isShowFollow = true;
                }
            }
            return View(upcoming);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}