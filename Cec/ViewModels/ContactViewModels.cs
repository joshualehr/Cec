using Cec.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class ContactIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public IList<ContactIndexItem> Contacts { get; set; }

        //Constructors
        public ContactIndexViewModel()
        {
            Contacts = db.Contacts.OrderBy(c => c.Company + c.LastName + c.FirstName)
                                  .Select(c => new ContactIndexItem { ContactId = c.ContactID, Company = c.Company, FullName = c.FirstName + " " + c.LastName, Title = c.Title })
                                  .ToList();
        }
    }

    public class ContactIndexByProjectViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public IList<ContactIndexItem> Contacts { get; set; }

        public Guid ProjectId { get; set; }

        public string Project { get; set; }

        [Required(ErrorMessage = "*Contact is required")]
        [Display(Name = "Contact")]
        public Guid ContactId { get; set; }

        //Constructors
        public ContactIndexByProjectViewModel() { }

        public ContactIndexByProjectViewModel(Guid projectId)
        {
            ProjectId = projectId;
            Project = db.Projects.Single(p => p.ProjectID == projectId).Designation;
            Contacts = db.Contacts.Where(c => c.Projects.Any(p => p.ProjectID == projectId))
                                  .OrderBy(c => c.Company + c.LastName + c.FirstName)
                                  .Select(c => new ContactIndexItem { ContactId = c.ContactID, Company = c.Company, FullName = c.FirstName + " " + c.LastName, Title = c.Title })
                                  .ToList();
        }

        //Methods
        public Guid AssociateContactWithProject(Guid projectId, Guid contactId)
        {
            db.ProjectContacts.Add(new ProjectContact {
                ProjectID = projectId,
                ContactID = contactId
            });
            db.SaveChanges();
            return projectId;
        }
    }

    public class ContactIndexItem
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid ContactId { get; set; }

        [DisplayName("Name")]
        public string FullName { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Company { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Title { get; set; }

        //Constructors
        public ContactIndexItem() { }

        public ContactIndexItem(Contact contact)
        {
            ContactId = contact.ContactID;
            FullName = contact.FirstName + " " + contact.LastName;
            Company = contact.Company;
            Title = contact.Title;
        }
    }

    public class ContactDetailsViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ContactId { get; set; }

        [DisplayName("Name")]
        public string FullName { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Company { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Title { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Trade { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(NullDisplayText = "-")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayFormat(NullDisplayText = "-")]
        public string Email { get; set; }

        //Constructors
        public ContactDetailsViewModel()
        {

        }

        public ContactDetailsViewModel(Guid contactId)
        {
            var contact = db.Contacts.Find(contactId);
            ContactId = contact.ContactID;
            FullName = contact.FirstName + " " + contact.LastName;
            Company = contact.Company;
            Title = contact.Title;
            Trade = contact.Trade;
            Phone = contact.Phone;
            Email = contact.Email;
        }    
    }

    public class ContactCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ContactId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Company { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Trade { get; set; }

        [Phone()]
        [DataType(DataType.PhoneNumber, ErrorMessage="Please enter a valid phone number.")]
        [StringLength(10, ErrorMessage = "Must be 10 digits.", MinimumLength = 10)]
        public string Phone { get; set; }

        [EmailAddress()]
        [DataType(DataType.EmailAddress, ErrorMessage="Please enter a valid email.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage="Please enter a vailid link address.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Chat { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage = "Please enter a vailid web address.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Website { get; set; }

        //Constructors
        public ContactCreateViewModel() { }

        public ContactCreateViewModel(Guid projectId) { }

        //Methods
        public Guid CreateContact()
        {
            var contact = new Contact() { 
                ContactID = Guid.Empty, 
                FirstName = this.FirstName, 
                LastName = this.LastName, 
                Company = this.Company, 
                Title = this.Title, 
                Trade = this.Trade, 
                Phone = this.Phone, 
                Email = this.Email, 
                Chat = this.Chat,
                Website = this.Website
            };
            db.Contacts.Add(contact);
            db.SaveChanges();
            return contact.ContactID;
        }        
    }

    public class ContactEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ContactId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Company { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Trade { get; set; }

        [Phone()]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(10, ErrorMessage = "Must be 10 digits.", MinimumLength = 10)]
        public string Phone { get; set; }

        [EmailAddress()]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage = "Please enter a vailid link address.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Chat { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage = "Please enter a vailid web address.")]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Website { get; set; }

        //Constructors
        public ContactEditViewModel()
        {

        }

        public ContactEditViewModel(Guid contactId)
        {
            var contact = db.Contacts.Find(contactId);
            this.ContactId = contact.ContactID;
            this.FirstName = contact.FirstName;
            this.LastName = contact.LastName;
            this.Company = contact.Company;
            this.Title = contact.Title;
            this.Trade = contact.Trade;
            this.Phone = contact.Phone;
            this.Email = contact.Email;
            this.Chat = contact.Chat;
            this.Website = contact.Website;
        }

        //Methods
        public Guid EditContact()
        {
            var contact = new Contact()
            {
                ContactID = this.ContactId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Company = this.Company,
                Title = this.Title,
                Trade = this.Trade,
                Phone = this.Phone,
                Email = this.Email,
                Chat = this.Chat,
                Website = this.Website
            };
            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();
            return this.ContactId;
        }
    }

    public class ContactSelectList : SelectList
    {
        //Constructors
        public ContactSelectList() : base(items(), "Value", "Text") { }

        public ContactSelectList(Guid projectId) : base(items(projectId), "Value", "Text") { }

        //Methods
        public static IEnumerable items()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Contacts.OrderBy(c => c.Company + c.LastName + c.FirstName)
                              .Select(c => new SelectListItem {
                                Value = c.ContactID.ToString(), 
                                Text = c.Company + ", " + c.FirstName + " " + c.LastName
                              });
        }

        public static IEnumerable items(Guid projectId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var currentContacts = db.ProjectContacts.Where(p => p.ProjectID == projectId).Select(p => p.Contact);
            return db.Contacts.Except(currentContacts)
                              .OrderBy(c => c.Company + c.LastName + c.FirstName)
                              .Select(c => new SelectListItem
                              {
                                  Value = c.ContactID.ToString(),
                                  Text = c.Company + ", " + c.FirstName + " " + c.LastName
                              });
        }
    }

    public class UserSelectList : SelectList
    {
        //Constructors
        public UserSelectList() : base(items(), "Value", "Text") { }

        public UserSelectList(string roleName) : base(items(roleName), "Value", "Text") { }

        //Methods
        public static IEnumerable items()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Users.Include(u => u.Contact)
                            .OrderBy(u => u.Contact.LastName + u.Contact.FirstName)
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.Contact.FirstName + " " + u.Contact.LastName
                            });
        }

        public static IEnumerable items(string roleName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            return db.Users.Include(u => u.Contact).Include(u => u.Roles)
                            .Where(u => u.Roles.Any(r => r.RoleId == roleManager.FindByName(roleName).Id))
                            .OrderBy(u => u.Contact.LastName + u.Contact.FirstName)
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.Contact.FirstName + " " + u.Contact.LastName
                            });
        }
    }
}