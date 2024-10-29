using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    [Table("file_metadata")]
    public class FileMetadata : AuditEntity
    {

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("object_key")]
        public string ObjectKey { get; set; }

        [Required]
        [Column("size")]
        public long Size { get; set; }

        [Required]
        [Column("content_type")]
        public string ContentType { get; set; }

        [Required]
        [Column("url")]
        public string Url { get; set; }

    }

}
