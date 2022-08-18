using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Entity.Model
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
      
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }
        public bool IsActive { get; set; }

    }
}
