using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("posts")]
    public class Post : AuditEntity<Guid>
    {

        public Post() => Id = Guid.NewGuid();

        [Required]
        [Column("content", TypeName = "jsonb")]
        public PostContent Content { get; set; } = new();

        [Required]
        [Column("type")]
        public PostType Type { get; set; } = PostType.Text;

        [Column("presentation_id")]
        public Guid? PresentationId { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; } // Nullable for shared posts

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; } // Link to Profle service

        [Column("group_id")]
        public Guid? GroupId { get; set; } // Link to Group service

        [Column("file_metadata_id")]
        public Guid? FileMetadataId { get; set; } // Link to File service

        [ForeignKey("ParentId")]
        public virtual Post Parent { get; set; }

        [ForeignKey("PresentationId")]
        public virtual Post Presentation { get; set; }

        public virtual ICollection<Post> SharedPosts { get; set; } = [];

        public virtual ICollection<Post> SubPosts { get; set; } = [];
        public virtual ICollection<PostReaction> PostReactions { get; set; } = [];

        public virtual PostAudience Audience { get; set; }

    }

}
