using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.BuildingBlocks.Domain.Entities
{
    public abstract class AuditEntity
    {
        [Column("created_by")]
        [MaxLength(255)]
        public string CreatedBy { get; set; } = null;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_by")]
        [MaxLength(255)]
        public string UpdatedBy { get; set; } = null;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_by")]
        [MaxLength(255)]
        public string DeletedBy { get; set; } = null;

        [Column("deleted_at")]
        public DateTime DeletedAt { get; set; } = new DateTime(9999, 12, 31);

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

    }
}
