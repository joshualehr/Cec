using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Cec.ViewModels
{
    public class ContactIndexViewModel
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
        public ContactIndexViewModel()
        {

        }

        public ContactIndexViewModel(Contact contact)
        {
            this.ContactId = contact.ContactID;
            this.FullName = contact.FirstName + " " + contact.LastName;
            this.Company = contact.Company;
            this.Title = contact.Title;
        }

        //Methods
        public List<ContactIndexViewModel> ListAll()
        {
            var contacts = db.Contacts.OrderBy(c => c.LastName)
                                      .OrderBy(c => c.FirstName);
            if (contacts.Count() > 0)
            {
                var contactIndexViewModels = new List<ContactIndexViewModel>();
                foreach (var item in contacts)
                {
                    var contactIndexViewModel = new ContactIndexViewModel(item);
                    contactIndexViewModels.Add(contactIndexViewModel);
                }
                return contactIndexViewModels;
            }
            else
	        {
                return null;
	        }
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

        [DataType(DataType.Url)]
        [DisplayFormat(NullDisplayText = "-")]
        public string Chat { get; set; }

        [DataType(DataType.Url)]
        [DisplayFormat(NullDisplayText = "-")]
        public string Website { get; set; }

        //Constructors
        public ContactDetailsViewModel()
        {

        }

        public ContactDetailsViewModel(Guid contactId)
        {
            var contact = db.Contacts.Find(contactId);
            this.ContactId = contact.ContactID;
            this.FullName = contact.FirstName + " " + contact.LastName;
            this.Company = contact.Company;
            this.Title = contact.Title;
            this.Trade = contact.Trade;
            this.Phone = contact.Phone;
            this.Email = contact.Email;
            this.Chat = contact.Chat;
            this.Website = contact.Website;
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
        public ContactCreateViewModel()
        {

        }

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

    public class ContactDeleteViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ContactId { get; set; }

        [DisplayName("Name")]
        public string FullName { get; set; }

        //Constuctors
        public ContactDeleteViewModel()
        {

        }

        public ContactDeleteViewModel(Guid contactId)
        {
            var contact = db.Contacts.Find(contactId);
            this.ContactId = contact.ContactID;
            this.FullName = contact.FirstName + " " + contact.LastName;
        }

        public void DeleteContact()
        {
            var contact = db.Contacts.Find(this.ContactId);
            db.Contacts.Remove(contact);
            db.SaveChanges();
        }
    }
}