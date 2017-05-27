namespace StringApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using StringApp.Data.Common;
    using StringApp.Data.Common.Models;

    public class User : IdentityUser, IHavePrimaryKey<string>, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}