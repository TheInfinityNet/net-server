﻿using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.Services.Comment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Comment.Domain.Entities
{
    [Table("comments")]
    public class Comment : AuditEntity<Guid>
    {
        public Comment() => Id = Guid.NewGuid();

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } // Link to Profile service

        [Required]
        [Column("post_id")]
        public Guid PostId { get; set; } // Link to Post service

        [Column("parent_id")]
        public Guid? ParentId { get; set; } = null; // Nullable for reply comments

        [Column("file_metadata_id")]
        public Guid? FileMetadataId { get; set; } = null; // Link to File service

        [Required]
        [Column("type")]
        public CommentType Type { get; set; }

        [Required]
        [Column("content", TypeName = "jsonb")]
        public CommentContent Content { get; set; } = new CommentContent();

        [ForeignKey("ParentId")]
        public virtual Comment ParentComment { get; set; } = null;

        public virtual ICollection<Comment> RepliesComments { get; set; } = [];

    }
}
