using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("posts")]
    public class Post : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("content")]
        public string Content { get; set; }

        [Required]
        [Column("privacy")]
        public Privacy Privacy { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; } // Nullable for shared posts

        [Required]
        [Column("post_type")]
        public PostType PostType { get; set; }

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; } // Link to Account service

        [Column("group_id")]
        public Guid? GroupId { get; set; } // Link to Group service

        [Column("media_id")]
        public Guid? MediaId { get; set; } // Link to File service
    }

}
