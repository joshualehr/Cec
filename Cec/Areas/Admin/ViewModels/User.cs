using Cec.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Areas.Admin.ViewModels
{
    public class RoleCheckBox
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    public class UserIndexViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
    }

    public class UserCreateViewModel
    {
        [Required, MaxLength(128), Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public IList<RoleCheckBox> Roles { get; set; }
    }

    public class UserEditViewModel
    {
        public string UserId { get; set; }

        [Required, MaxLength(128), Display(Name = "User Name")]
        public string UserName { get; set; }

        public IList<RoleCheckBox> Roles { get; set; }
    }

    public class UserResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Order = 1)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "* Does not match")]
        [Display(Name = "Confirm", Prompt = "Confirm", Order = 2)]
        public string ComfirmPassword { get; set; }
    }

    public class UserDeleteViewModel
    {
        public string UserId { get; set; }
    }
}