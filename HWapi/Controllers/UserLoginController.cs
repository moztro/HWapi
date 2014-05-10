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
    public class UserLoginController : ApiController
    {
        private HWEntities db = new HWEntities();

        // GET api/UserLogin
        public IEnumerable<User> GetUsers()
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.MethodNotAllowed));
        }

        // GET api/UserLogin/5
        public User GetUser(int id)
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.MethodNotAllowed));
        }

        // PUT api/UserLogin/5
        public HttpResponseMessage PutUser(int id, User user)
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.MethodNotAllowed));
        }

        // POST api/UserLogin
        /// <summary>
        /// Validate User credentials
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User PostUser(User user)
        {
            return UserServices.GetInstance().ValidateCredentials(user);
        }

        // DELETE api/UserLogin/5
        public HttpResponseMessage DeleteUser(int id)
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.MethodNotAllowed));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}