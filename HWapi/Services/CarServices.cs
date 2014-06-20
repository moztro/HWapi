using HWapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWapi.Services
{
    public class CarServices
    {
        private static CarServices instance = null;
        private HWEntities db = new HWEntities();
        static IEnumerable<Car> CacheCar {get;set;}

        private CarServices() { }

        public static CarServices GetInstance()
        {
            if (instance == null)
                instance = new CarServices();
            return instance;
        }

        public IEnumerable<Car> GetCars()
        {
            return
                from c in db.Cars as IEnumerable<Car>
                where c.Activo.Equals(true)
                select new Car
                {
                    CarId = c.CarId,
                    CarName = c.CarName,
                    Tags = c.Tags
                };
        }

        public Car GetCar(int carId)
        {
            Car car = db.Cars.Find(carId);

            if (car == null)
                return null;

            return new Car()
            {
                CarId = car.CarId,
                CarName = car.CarName,
                Tags = car.Tags,
                Activo = car.Activo
            };
        }

        public IEnumerable<CarCollection> GetCollections()
        {
            return
                db.CarCollections
                .Where(c => c.Activo.Equals(true))
                .Select(c => new CarCollection 
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
                }).ToList();
        }

        public CarCollection GetCollection(int collectionId)
        {
            CarCollection carCollection = db.CarCollections.Find(collectionId);

            if (carCollection == null)
                return null;

            return new CarCollection()
            {
                CarCollectionId = carCollection.CarCollectionId,
                CarId = carCollection.CarId,
                Car = new Car
                {
                    CarId = carCollection.Car.CarId,
                    CarName = carCollection.Car.CarName,
                    Tags = carCollection.Car.Tags
                },
                UserId = carCollection.UserId,
                User = new User
                {
                    UserId = carCollection.User.UserId,
                    UserName = carCollection.User.UserName,
                    UserLogin = carCollection.User.UserLogin
                }
            };
        }
    }
}