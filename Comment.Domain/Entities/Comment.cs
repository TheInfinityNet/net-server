using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Comment.Domain.Entities
{
    [Table("comments")]
    public class Comment : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("post_id")]
        public Guid PostId { get; set; } // Link to Post service

        [Column("parent_id")]
        public Guid? ParentId { get; set; } = null; // Nullable for reply comments

        [Column("media_id")]
        public Guid? MediaId { get; set; } = null; // Link to File service

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [ForeignKey("ParentId")]
        public virtual Comment? ParentComment { get; set; } = null;

        public virtual ICollection<Comment> repliesComments { get; set; } = new List<Comment>();

    }
}
