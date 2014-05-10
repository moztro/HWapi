using HWapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HWapi.Services
{
    public class UserServices
    {
        private HWEntities db = new HWEntities();
        private static UserServices instance = null;

        private UserServices() { }

        public static UserServices GetInstance()
        {
            if (instance == null)
                instance = new UserServices();
            return instance;
        }

        public IEnumerable<User> GetUsers()
        {
            return
                from u in db.Users as IEnumerable<User>
                where u.Activo.Equals(true)
                select new User
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    UserLogin = u.UserLogin,
                    UserPassword = u.UserPassword,
                    Activo = u.Activo,
                    CarCollections = u.CarCollections
                        .Where(cc => cc.Activo.Equals(true))
                        .Select(c => new CarCollection
                        {
                            CarCollectionId = c.CarCollectionId,
                            CarId = c.CarId
                        }).ToList()
                };
        }

        public User GetUser(int id)
        {
            User user = db.Users.Find(id);

            if (user == null)
                return null;

            return new User()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserLogin = user.UserLogin,
                Activo = user.Activo,
                CarCollections = user.CarCollections
                    .Select(cc => new CarCollection
                    {
                        CarCollectionId = cc.CarCollectionId,
                        CarId = cc.CarId
                    })
                    .ToList()
            };
        }

        public int Save(User user)
        {
            db.Users.Add(user);
            return db.SaveChanges();
        }

        public int Update(User user)
        {
            db.Entry(user).State = System.Data.EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(User user)
        {
            db.Users.Remove(user);
            return db.SaveChanges();
        }

        public User ValidateCredentials(User user)
        {
            User usrLogin = db.ValidateLogin(user.UserLogin, user.UserPassword)
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserLogin = u.UserLogin,
                    UserName = u.UserName
                })
                .SingleOrDefault();

            return usrLogin;
        }
    }
}