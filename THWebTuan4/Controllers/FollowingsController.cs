using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using THWebTuan4.Models;

namespace THWebTuan4.Controllers
{
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Please login first");
            if (userID == follow.FolloweeId)
                return BadRequest("Cannot follow myself");
            DBConnect db = new DBConnect();
            Following find = db.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);
            if (find != null)
            {
                db.Followings.Remove(db.Followings.Single(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                db.SaveChanges();
                return Ok("cancel");
            }
            follow.FollowerId = userID;
            db.Followings.Add(follow);
            db.SaveChanges();

            return Ok();
        }
    }
}
