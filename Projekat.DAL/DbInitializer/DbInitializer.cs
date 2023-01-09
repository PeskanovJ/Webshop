using Microsoft.EntityFrameworkCore;
using Projekat.DAL.Model;
using Projekat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            if (_db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com") == null)
            {
                byte[] salt = PasswordHasher.GenerateSalt();
                _db.Users.Add(new Model.User
                {
                    
                    
                    ActivationGuid = Guid.Empty,
                    PasswordGuid = Guid.Empty,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Created = DateTime.Now,
                    Email = "admin@gmail.com",
                    FollowedItems = new List<Following>(),
                    PhoneNumber = "000000",
                    Messages = new List<Message>(),
                    Password = PasswordHasher.GenerateSaltedHash(Encoding.ASCII.GetBytes("Admin12345"), salt),
                    PostedItems = new List<Item>(),
                    ProfileUrl = @"\img\profilePictures\img_avatar.png",
                    Role = Shared.Constants.SD.Roles.Admin,
                    Salt = salt,

                });
                if (_db.Categories.FirstOrDefault(u => u.Name == "eSkuteri") == null)
                {
                    _db.Categories.Add(new Model.Category { Name = "eSkuteri" });
                }
                if (_db.Categories.FirstOrDefault(u => u.Name == "eTrotineti") == null)
                {
                    _db.Categories.Add(new Model.Category {  Name = "eTrotineti" });
                }
                if (_db.Categories.FirstOrDefault(u => u.Name == "eBicikli") == null)
                {
                    _db.Categories.Add(new Model.Category {  Name = "eBicikli" });
                }

                _db.SaveChanges();

            }
            return;
        }
    }
}
