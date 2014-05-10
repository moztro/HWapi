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
    public class CarController : ApiController
    {
        private HWEntities db = new HWEntities();
        //private IEnumerable<Car> CacheCar { get; set; }
        private IEnumerable<Car> CacheCar;

        // GET api/Car
        public IEnumerable<Car> GetCars()
        {
            if (CacheCar != null)
                return CacheCar;

            CacheCar = CarServices.GetInstance().GetCars();

            return CacheCar;
        }

        // GET api/Car/5
        public Car GetCar(int id)
        {
            if (CacheCar == null)
                return CarServices.GetInstance().GetCar(id);

            Car car = null;
            foreach (Car c in CacheCar)
            {
                if (c.CarId.Equals(id))
                {
                    car = c;
                    break;
                }
            }

            return car;
        }

        // PUT api/Car/5
        public HttpResponseMessage PutCar(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != car.CarId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(car).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                CacheCar = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Car
        public HttpResponseMessage PostCar(Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                CacheCar = null;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, car);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = car.CarId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Car/5
        public HttpResponseMessage DeleteCar(int id)
        {
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Cars.Remove(car);

            try
            {
                db.SaveChanges();
                CacheCar = null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, car);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}