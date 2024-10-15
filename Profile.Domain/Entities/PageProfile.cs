using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("page_profiles")]
    public class PageProfile : Profile
    {

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; }

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [ForeignKey("Id")]
        public virtual Profile Profile { get; set; }

    }

}
