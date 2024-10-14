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

        [Column("page_name")]
        [MaxLength(100)]
        [Required]
        public string PageName { get; set; } = string.Empty;

        [Column("page_description", TypeName = "text")]
        public string PageDescription { get; set; }

        [ForeignKey("Id")]
        public virtual Profile Profile { get; set; }

    }

}
