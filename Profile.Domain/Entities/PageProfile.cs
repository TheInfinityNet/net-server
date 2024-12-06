using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("page_profiles")]
    public class PageProfile : Profile
    {

        public PageProfile() => Type = ProfileType.Page;

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [ForeignKey("Id")]
        public virtual Profile Profile { get; set; }

    }

}
