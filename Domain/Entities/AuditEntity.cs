using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.BuildingBlocks.Domain.Entities
{
    public abstract class AuditEntity
    {
        [Column("created_by")]
        [MaxLength(255)]
        public Guid? CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_by")]
        [MaxLength(255)]
        public Guid? UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_by")]
        [MaxLength(255)]
        public Guid? DeletedBy { get; set; }

        [Column("deleted_at")]
        public DateTime DeletedAt { get; set; } = new DateTime(9999, 12, 31);

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

    }
}
