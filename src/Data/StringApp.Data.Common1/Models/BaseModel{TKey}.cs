﻿namespace StringApp.Data.Common.Models
{
    using System;

    public abstract class BaseModel<TKey> : IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Key]
        public TKey Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}