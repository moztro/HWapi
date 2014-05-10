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
    public class CollectionController : ApiController
    {
        private HWEntities db = new HWEntities();
        private IEnumerable<CarCollection> CacheCollection { get; set; }

        // GET api/Collection
        /// <summary>
        /// Get all car collections stored
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CarCollection> GetCarCollections()
        {
            if (CacheCollection != null)
                return CacheCollection;

            CacheCollection =
                from c in db.CarCollections
                         .Include(c => c.Car)
                         .Include(c => c.User)
                         as IEnumerable<CarCollection>
                where c.Activo.Equals(true)
                select new CarCollection
                {
                    CarCollectionId = c.CarCollectionId,
                    CarId = c.CarId,
                    Car = new Car
                    {
                        CarId = c.Car.CarId,
                        CarName = c.Car.CarName,
                        Tags = c.Car.Tags
                    },
                    UserId = c.UserId,
                    User = new User
                    {
                        UserId = c.User.UserId,
                        UserName = c.User.UserName,
                        UserLogin = c.User.UserLogin
                    }
                };

            return CacheCollection;
        }

        // GET api/Collection/5
        /// <summary>
        /// Get a car collection by a unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarCollection GetCarCollection(int id)
        {
            if (CacheCollection == null)
                return CarServices.GetInstance().GetCollection(id);

            CarCollection collection = null;
            foreach (CarCollection cc in CacheCollection)
            {
                if (cc.CarCollectionId.Equals(id))
                {
                    collection = cc;
                    break;
                }
            }
            return collection;
        }

        // PUT api/Collection/5
        /// <summary>
        /// Modifies an existing collection 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="carCollection"></param>
        /// <returns></returns>
        public HttpResponseMessage PutCarCollection(int id, CarCollection carCollection)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != carCollection.CarCollectionId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(carCollection).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                CacheCollection = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Collection
        /// <summary>
        /// Save a new car collection object in database
        /// </summary>
        /// <param name="carCollection"></param>
        /// <returns></returns>
        public HttpResponseMessage PostCarCollection(CarCollection carCollection)
        {
            if (ModelState.IsValid)
            {
                db.CarCollections.Add(carCollection);
                db.SaveChanges();
                CacheCollection = null;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, carCollection);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = carCollection.CarCollectionId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Collection/5
        /// <summary>
        /// Deletes an existing car collection in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage DeleteCarCollection(int id)
        {
            CarCollection carCollection = db.CarCollections.Find(id);
            if (carCollection == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CarCollections.Remove(carCollection);

            try
            {
                db.SaveChanges();
                CacheCollection = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, carCollection);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}