using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.Areas.Admin.ViewModels
{
    public class RoleCheckBox
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int WorkFactor = 13;

        public static void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("", WorkFactor);
        }

        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PasswordHash { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }

        public User()
        {
            Roles = new List<AspNetRole>();
        }

        public User(string id)
        {
            var user = db.AspNetUsers.Find(id);
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.PasswordHash = user.PasswordHash;
            this.Roles = user.AspNetRoles;
        }

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public virtual bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }

    //GET: /Admin/User
    public class UserIndexViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<AspNetUser> Users { get; set; }

        public UserIndexViewModel()
        {
            this.Users = db.AspNetUsers;
        }
    }

    //GET: /Admin/User/Create
    public class UserCreateViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IList<RoleCheckBox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public UserCreateViewModel()
        {
            var Roles = db.AspNetRoles.Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList();
        }

        public string Create()
        {
            var user = new User();
            SyncRoles(this.Roles, user.Roles);

            if (db.AspNetUsers.Any(u => u.UserName == this.UserName))
                ModelState.AddModelError("UserName", "UserName must be unique.");

            user.UserName = this.UserName;
            user.SetPassword(this.Password);

            db.SaveChanges();

            return this.UserName;
        }

        private void SyncRoles(IList<RoleCheckBox> checkboxes, ICollection<AspNetRole> roles)
        {
            var selectedRoles = new List<AspNetRole>();

            foreach (var role in db.AspNetRoles)
            {
                var checkbox = checkboxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;

                if (checkbox.IsChecked)
                    selectedRoles.Add(role);
            }

            foreach (var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
                roles.Add(toAdd);

            foreach (var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
                roles.Remove(toRemove);
        }
    }

    //GET: /Admin/User/Edit/5
    public class UserEditViewModel
    {
        public IList<RoleCheckBox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }
    }

    public class UserResetPasswordViewModel
    {
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}