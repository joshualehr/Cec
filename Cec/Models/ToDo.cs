using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class ToDo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ToDoID { get; set; }

        public virtual ICollection<ApplicationUser> AssignedTo { get; set; }

        public virtual ICollection<ToDo> ChildToDos { get; set; }

        [Required()]
        [Index(IsClustered = false, IsUnique = false)]
        public Guid AreaID { get; set; }

        public virtual Area Area { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        public Guid? ParentToDoID { get; set; }

        public virtual ToDo ParentToDo { get; set; }

        [Required]
        [StringLength(20)]
        public string Heading { get; set; }

        [Required()]
        [StringLength(200)]
        public string Description { get; set; }

        public DateTime? StartOn { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletedOn { get; set; }

        public short ListOrder { get; set; }
    }
}