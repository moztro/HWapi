using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using HWapi.Models;
using HWapi.Services;

namespace HWapi.Controllers
{
    public class UserController : ApiController
    {
        private HWEntities db = new HWEntities();
        private IEnumerable<User> CacheUsers { get; set; }

        // GET api/User
        public IEnumerable<User> GetUsers()
        {
            if (CacheUsers != null)
                return CacheUsers;

            CacheUsers = UserServices.GetInstance().GetUsers();

            return CacheUsers;
        }

        // GET api/User/5
        public User GetUser(int id)
        {
            if (CacheUsers == null)
                return UserServices.GetInstance().GetUser(id);

            User user = null;
            foreach (User u in CacheUsers)
            {
                if (u.UserId.Equals(id))
                {
                    user = u;
                    break;
                }
            }
            return user;
        }

        // PUT api/User/5
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.UserId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                CacheUsers = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/User
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                UserServices.GetInstance().Save(user);
                CacheUsers = null;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/User/5
        public HttpResponseMessage DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
                CacheUsers = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}