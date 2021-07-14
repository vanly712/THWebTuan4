using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THWebTuan4.Models;

namespace THWebTuan4.Controllers
{
    public class CourseController : Controller
    {
        DBConnect context = new DBConnect();
        // GET: Course
        public ActionResult Create()
        {
            
            //cái này là cái ado.net e đặt là DBConnect nên phải để nó là DBConnect
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create( Course objCourse)
        {
            ModelState.Remove("LecturedId");
            if (ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int? id)
        {
            Course course = context.Courses.Find(id);
            course.ListCategory = context.Categories.ToList();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course objCourse)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            if (ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                context.Entry(objCourse).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int? id)
        {
            Course course = context.Courses.Find(id);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Course course = context.Courses.Find(id);
            Attendance attendance = context.Attendances.Find(id, user.Id);
            context.Attendances.Remove(attendance);
            context.SaveChanges();
            context.Courses.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == user.Id).ToList();
            var course = new List<Course>();
            foreach (Attendance attendance in listAttendances)
            {
                Course objCourse = attendance.Course;
                objCourse.LecturerId = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                course.Add(objCourse);
            }
            return View(course);
        }

        public ActionResult Mine()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var course = context.Courses.Where(c => c.LecturerId == user.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course c in course)
            {
                c.LecturerId = user.Name;
            }
            return View(course);
        }
    }
}