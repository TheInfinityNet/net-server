using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("posts")]
    public class Post : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Column("privacy")]
        public Privacy Privacy { get; set; }

        [Required]
        [Column("post_type")]
        public PostType Type { get; set; } = PostType.Text;

        [Column("presentation_id")]
        public Guid? PresentationId { get; set; } = null;

        [Column("parent_id")]
        public Guid? ParentId { get; set; } = null; // Nullable for shared posts

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; } // Link to Profle service

        [Column("group_id")]
        public Guid? GroupId { get; set; } = null; // Link to Group service

        [Column("media_id")]
        public Guid? MediaId { get; set; } = null; // Link to File service

        [ForeignKey("ParentId")]
        public virtual Post Parent { get; set; } = null;

        [ForeignKey("PresentationId")]
        public virtual Post Presentation { get; set; } = null;

        public virtual ICollection<Post> SharedPosts { get; set; } = new List<Post>();

        public virtual ICollection<Post> SubPosts { get; set; } = new List<Post>();

    }

}
